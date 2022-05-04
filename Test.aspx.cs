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


public partial class Test : System.Web.UI.Page
{
    private static HttpClient _client = new HttpClient();
    private DataSet _forecastData = new DataSet();
    private string _json;
    protected async void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            // LoadJson(); // For offline testing
            InitializeData(await GetForecastHourly());
        }
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

    protected void InitializeData(string jsonData)
    {
        // Take the raw JSON string and separate it into DataTables
        // Add the DataTables to a DataSet        

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

            // Bind all
            gridCoordinates.DataSource = dtCoordinates;
            gridProperties.DataSource = dtProperties;
            gridPeriods.DataSource = dtPeriods;
            
            gridCoordinates.DataBind();
            gridProperties.DataBind();
            gridPeriods.DataBind();
            

        }
        catch (Exception err)
        {
            lblStatus.Text = err.Message;
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