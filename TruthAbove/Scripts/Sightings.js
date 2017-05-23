$(document).ready(function () {

    var obj = new SightingJS();
    obj.Initialize();
});

function filterSightings (e) {
    var tbl = $("#sightingsTable").DataTable();

    $.get("Home/FilterSightings", { state: $("#searchState").val(), shape: $("#searchShape").val() }
        , function (filteredSightings) {
        tbl.clear();
        tbl.rows.add(filteredSightings);
        tbl.draw();
    });
};


function SightingJS() {

    var self = this;

    self.dataColumns = [
                    {
                        title: "Incident Date", name: "Incident Date", data: 'IncidentDate', dataType: 'date'
                        , render: function (d) {
                            return new Date(d.match(/\d+/)[0] * 1).toLocaleDateString();
                        }
                    },
                    { title: "City", name: "City", data: 'City', dataLength: 30 },
                    { title: "State", name: "State", data: 'State', dataLength: 2 },
                    { title: "Shape", name: "Shape", data: 'Shape', dataLength: 20 },
                    { title: "Duration", name: "Duration", data: 'Duration', dataLength: 12 },
                    { title: "Summary", name: "Summary", data: 'Summary', dataLength: 200},
                    {
                        title: "Posted", name: "Posted", data: 'PostedDate', dataLength: 12
                        , render: function (d) {
                            return new Date(d.match(/\d+/)[0] * 1).toLocaleDateString();
                        }
                    }
    ];

    this.Initialize = function () {

        self.LoadSightings();

        var editTable = $("");

        $(self.dataColumns).each(function (index, column) {
            var colLabel = "<td class='columnLabel'>" + column.title + ":</td>";
            var colInput = "<td class='columnInput'>" 
                + "<input type='text' size='" + column.dataLength + "' name='"  + column.data + "' />" 
                + "</td>";

            $("#addSightingModal tbody").append("<tr>" + colLabel + colInput + "</tr>");
        });


        $(".addSighting button").click(function () {
            $("#addSightingModal").modal();

        });

        $(".saveSightingsButton").click(function () {
            self.addSighting();
        });
    };
    
    this.addSighting = function () {

        var cols = {};
        $(self.dataColumns).each(function (index, column) {
            var val = $("#addSightingModal [name='" + column.data + "']").val();
            cols[column.name] = val;
        });

        $.post("Home/SaveSighting", cols, function (resp) {

            $("#addSightingModal button").attr("disabled", "disabled");

            $("#addSightingModal .saveMessage").html("Sighting Saved!");

            setTimeout(function () {
                $("#addSightingModal").modal('hide');
                $("#addSightingModal button").removeAttr("disabled");
                $("#addSightingModal .saveMessage").html("");
            }, 2000);

        });

    };

    this.LoadSightings = function () {

        $.get("Home/GetSightings", function (sightings) {
            
            // using datatables.net
            this.sightingsTable = $("#sightingsTable").DataTable({
                bFilter: false,
                paginate: true,       
                paging: true,
                fixedHeader: {
                    header: true
                },
                data: sightings,
                columns: self.dataColumns

            });
        });

    };
    
}