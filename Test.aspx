<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript"> 
        function display_c() {
            var refresh = 1000; // Refresh rate in milli seconds
            mytime = setTimeout('display_ct()', refresh)
        }

        function display_ct() {
            var x = new Date()
            document.getElementById('clock').innerHTML = x.toLocaleTimeString();
            document.getElementById('date').innerHTML = x.toLocaleDateString("en-US", { dateStyle: "medium" });
            document.getElementById('weekday').innerHTML = x.toLocaleDateString('us-EN', { weekday: 'long' });
            display_c();
        }
    </script>
</head>
<body onload="display_ct();">
    <form id="form1" runat="server">
        <div id="time">
            <span id='weekday'></span>
            <br />
            <br />
            <span id='date'></span>
            <br />
            <br />
            <span id='clock'></span>
        </div>
        <br />
        <br />
        <div id="weather">
            <asp:GridView ID="gridCoordinates" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="lat" HeaderText="Latitude" />
                    <asp:BoundField DataField="long" HeaderText="Longitude" />
                </Columns>
            </asp:GridView>
            <br />
            <br />
            <asp:GridView ID="gridProperties" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="updated" HeaderText="Updated" />
                    <asp:BoundField DataField="elevation" HeaderText="Elevation" />
                </Columns>
            </asp:GridView>
            <br />
            <br />
            <asp:GridView ID="gridPeriods" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="number" HeaderText="#" />
                    <asp:BoundField DataField="startTime" HeaderText="Start Time" />
                    <asp:BoundField DataField="endTime" HeaderText="End Time" />
                    <asp:BoundField DataField="isDaytime" HeaderText="Daytime?" />
                    <asp:BoundField DataField="temperature" HeaderText="Temperature (F)" />
                    <asp:BoundField DataField="windSpeed" HeaderText="Wind Speed" />
                    <asp:BoundField DataField="windDirection" HeaderText="Wind Direction" />
                    <asp:BoundField DataField="icon" HeaderText="Icon" Visible="false" />
                    <asp:TemplateField HeaderText="Icon">
                        <ItemTemplate>
                            <img src="<%# Eval("icon") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="shortForecast" HeaderText="Short Forecast" />
                </Columns>
            </asp:GridView>
            <br />
            <br />
            <asp:GridView ID="gridDataSet" runat="server"></asp:GridView>
        </div>
        <div id="misc">
            
        </div>
        <br />
        <br />
        <asp:Label ID="lblStatus" runat="server"></asp:Label>
    </form>
</body>
</html>
