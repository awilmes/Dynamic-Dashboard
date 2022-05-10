using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using nwsAPI;


public partial class Display : System.Web.UI.Page
{
    private static HttpClient _client = new HttpClient();
    private DataSet _forecastData = new DataSet();
    private string _json;
    protected async void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LoadJson(); // For offline testing
            InitializeData(_json);
            GetRowsByFilter();
            Location location = AverageLatLong();
            lblLocation.Text += location.latitude.ToString() + ", " + location.longitude.ToString();
            lblElevation.Text += _forecastData.Tables["dtProperties"].Rows[0]["elevation"];
            lblUpdated.Text += _forecastData.Tables["dtProperties"].Rows[0]["updated"];
            
            //InitializeData(await GetForecastHourly()); // For online testing
            //GetRowsByFilter();
        }
        else
        {
            _forecastData = (DataSet)ViewState["forecastData"];
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ViewState["forecastData"] = _forecastData;
    }

    //----- FOR TESTING OFFLINE -----//
    public void LoadJson()
    {
        using (StreamReader r = new StreamReader("C:/Training/self/API/dynamic_display/App_Code/test_nws_json.json"))
        {
            _json = r.ReadToEnd();
        }
    }
    //----- FOR TESTING OFFLINE -----//
    
    private void GetRowsByFilter()
    {        
        // Filters the forecast DataTable (dtPeriods) to display current weather conditions

        DataTable source = _forecastData.Tables["dtPeriods"].Copy(); // Get the forecast table from the main DataSet
        DataTable result = source.Clone(); // Create a new table to store results and clone the schema from the source              

        for (int i = 0; i < source.Rows.Count; i++)
        {
            DataRow row = source.Rows[i];

            // Get the row within the current time frame
            if (Convert.ToDateTime(row["startTime"]) <= DateTime.Now && Convert.ToDateTime(row["endTime"]) >= DateTime.Now)
            {
                result.Rows.Add(row.ItemArray);                
                row = source.Rows[i + 1]; // Incrementing the index by 1 adds the next forecast period
                result.Rows.Add(row.ItemArray);
            }            
        }

        //gridPeriods.DataSource = result;
        //gridPeriods.DataBind();

        lblTemp.Text += result.Rows[0]["temperature"].ToString() + " °F";
        lblWind.Text += result.Rows[0]["windSpeed"].ToString() + " " + result.Rows[0]["windDirection"].ToString();
        imgIcon.ImageUrl = result.Rows[0]["icon"].ToString();
    }

    private Location AverageLatLong()
    {
        // Averages all the latitude and longitude points from the dtCoordinates table and returns a single pair of coordinates.
        
        List<decimal> lats = new List<decimal>();
        List<decimal> longs = new List<decimal>();

        DataTable points = _forecastData.Tables["dtCoordinates"].Copy();        

        for (int i = 0; i < points.Rows.Count; i++)
        {
            lats.Add(Convert.ToDecimal(points.Rows[i]["lat"]));
            longs.Add(Convert.ToDecimal(points.Rows[i]["long"]));
        }

        if (lats.Count > 0 && longs.Count > 0)
        {
            decimal avgLats = 0;
            decimal avgLongs = 0;

            foreach (decimal lat in lats)
            {
                avgLats += lat;
            }
            foreach (decimal lon in longs) 
            {
                avgLongs += lon;
            }

            avgLats = avgLats / lats.Count;
            avgLongs = avgLongs / longs.Count;            

            Location location = new Location();
            location.latitude = avgLats;
            location.longitude = avgLongs;

            return location;
        }
        return null;
    }

    protected void InitializeData(string jsonData)
    {
        // Take the raw JSON string and separate it into DataTables
        // Add the DataTables to a DataSet        

        if (jsonData != null)
        {
            // COLLECT: Properties/Periods, Geometry/Coordinates

            try
            {
                // Root object
                Rootobject forecastObject = JsonConvert.DeserializeObject<Rootobject>(jsonData);

                // Create the first table
                // ForecastHourly/Geometry/Coordinates
                Geometry geometry = forecastObject.geometry;
                DataTable dtCoordinates = new DataTable();

                dtCoordinates.Columns.Add("long");
                dtCoordinates.Columns.Add("lat");

                foreach (var coor in geometry.coordinates[0])
                {
                    DataRow row = dtCoordinates.NewRow();

                    for (int i = 0; i < coor.Length; i++)
                    {
                        row[i] = coor[i];
                    }

                    dtCoordinates.Rows.Add(row);
                }

                // Add the populated coordinates table to the DataSet
                _forecastData.Tables.Add(dtCoordinates);

                // Create the next table
                // ForecastHourly/Properties
                Properties properties = forecastObject.properties;
                DataTable dtProperties = new DataTable();

                dtProperties.Columns.Add("updated");
                dtProperties.Columns.Add("elevation");

                DataRow dataRow = dtProperties.NewRow();
                dataRow["updated"] = properties.updated;
                dataRow["elevation"] = properties.elevation.value + "m";
                dtProperties.Rows.Add(dataRow);

                _forecastData.Tables.Add(dtProperties);

                // Create the last table
                // ForecastHourly/Properties/Periods
                DataTable dtPeriods = new DataTable();
                dtPeriods.Columns.Add("number");
                //dtPeriods.Columns.Add("name");
                dtPeriods.Columns.Add("startTime");
                dtPeriods.Columns.Add("endTime");
                dtPeriods.Columns.Add("isDaytime");
                dtPeriods.Columns.Add("temperature");
                //dtPeriods.Columns.Add("temperatureUnit");
                //dtPeriods.Columns.Add("temperatureTrend");
                dtPeriods.Columns.Add("windSpeed");
                dtPeriods.Columns.Add("windDirection");
                dtPeriods.Columns.Add("icon");
                dtPeriods.Columns.Add("shortForecast");
                //dtPeriods.Columns.Add("detailedForecast");
                foreach (var period in properties.periods)
                {
                    Period p = period;
                    DataRow row = dtPeriods.NewRow();

                    row[0] = p.number;
                    //row[1] = p.name; // remove
                    row[1] = p.startTime;
                    row[2] = p.endTime;
                    row[3] = p.isDaytime;
                    row[4] = p.temperature;
                    //row[5] = p.temperatureUnit;
                    //row[7] = p.temperatureTrend; // remove
                    row[5] = p.windSpeed;
                    row[6] = p.windDirection;
                    row[7] = p.icon; // remove? see if can display image
                    row[8] = p.shortForecast;
                    //row[12] = p.detailedForecast; // remove

                    dtPeriods.Rows.Add(row);
                }

                _forecastData.Tables.Add(dtPeriods);

                // Adjust table names in DataSet
                _forecastData.Tables[0].TableName = "dtCoordinates";
                _forecastData.Tables[1].TableName = "dtProperties";
                _forecastData.Tables[2].TableName = "dtPeriods";

                // Bind all
                //gridCoordinates.DataSource = dtCoordinates;
                //gridProperties.DataSource = dtProperties;
                //gridPeriods.DataSource = dtPeriods;

                //gridCoordinates.DataBind();
                //gridProperties.DataBind();
                //gridPeriods.DataBind();


            }
            catch (Exception err)
            {
                lblStatus.Text = err.Message;
            }
        }
        else
        {
            // handle null data
            lblStatus.Text = "JSON was null!";
        }
        
    }

    public static async Task<string> GetForecastHourly()
    {
        // Gets the JSON string for hourly forecast data from the National Weather Service API
        // !!-REQUIRES USER-AGENT HEADER-!!

        // Define User-Agent header
        var productValue = new ProductInfoHeaderValue("TestWeatherBot", "1.0");
        var commentValue = new ProductInfoHeaderValue("(Student Contact Email: wilmez8@gmail.com)");
        // Define base URL
        string baseURL = "https://api.weather.gov/gridpoints/OUN/95,94/forecast/hourly";
        string output;

        try
        {
            // Add the User-Agent header to the HttpClient
            _client.DefaultRequestHeaders.UserAgent.Add(productValue);
            _client.DefaultRequestHeaders.UserAgent.Add(commentValue);

            using (_client)
            {
                using (HttpResponseMessage res = await _client.GetAsync(baseURL))
                {
                    using (HttpContent content = res.Content)
                    {
                        var data = await content.ReadAsStringAsync();

                        if (data != null)
                        {
                            output = data;
                            return output;
                        }
                        else
                        {
                            output = "Data was null";
                            return output;
                        }
                    }
                }
            }
        }
        catch (Exception err)
        {
            return err.Message;
        }
    }
}
