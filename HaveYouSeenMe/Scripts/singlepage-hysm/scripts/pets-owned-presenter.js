define(
    ["view-renderer", 
    "pet-details-presenter",
    "pet-edit-presenter", 
    "pet-create-presenter"], 
    function(renderer, petDetails, petEdit, petCreate) {

        var petsList = {}, 
            table,
            view = "pets-owned-view",
            mainContainer;

        function renderView(){
            renderer.renderTemplate(view, mainContainer, null);            
        }

        function setDataTable(){    
            table = $("#data-grid").dataTable({
                "bServerSide": true,
                "sAjaxSource": "PetsOwnedAjaxHandler",       
                "bProcessing": true,    	
                bJQueryUI: true,
                sPaginationType: "full_numbers",
                "aoColumns": [                         
                                { 
                                  "data": null, 
                                  "bSortable" : false,
                                  mRender : function( data, type, row ) { 
                                            petsList[row.PetID] = row;
                                            return '<img src="/Content/Uploads/thumbnail_' + 
                                            row.PetName +'.jpg" style="width:60px;height:60px" />'; 
                                  } 
                                },
                                { "data": "PetName" },
                                { "data": "PetAge", "sSortDataType": "dom-text", "sType": "numeric" }, 
                                { "data": "LastSeenOnFormated", 
                                  "sSortDataType": "dom-text", 
                                  "sType": "date",
                                  mRender : function( data, type, row ) {
                                    return row.LastSeenOnFormated.split("-").reverse().join("/");
                                  } 
                                },
                                { "data": "LastSeenWhere" },
                                { "data": "StatusDescription" },
                                { "data": null, 
                                  "bSortable": false, 
                                  mRender : function( data, type, row ) { 
                                      return '<p><a href="#" data-oper="edit" data-pet="'+row.PetID+'">Edit</a></p>'+ 
                                       '</p><a href="#" data-oper="details" data-pet="'+row.PetID+'">Details</a></p>';                                     
                                } },                        
                ],
                "sDom": '<"top">frt<"bottom"p><"clear">',
                "fnInitComplete": function () {
                    var api = this.api();
                    //console.log(api.data());
                    // api.$('td').click( function () {
                    //     console.log(api.data());
                    // } );
                }        
            });
        }

        function attachHandlers(){
            table.on( 'xhr', function () {
                var json = table.ajax.json();
                console.log( json.data.length +' row(s) were loaded' );
            } );

            //edit or details presenter
            $('#data-grid').on( 'click', 'td a', function (e) {
                e.preventDefault();
                var element = e.target;
                if(element.dataset.oper == "edit"){
                    callPetEdit(petsList[element.dataset.pet]);
                }else if (element.dataset.oper == "details"){
                    callPetDetails(petsList[element.dataset.pet]);
                }
            } );

            //create presenter    
            $('#add-new-pet').on('click', function(e){
                e.preventDefault();
                callPetCreate();
            });
        }

        // call pet details presenter, which shows pet details view
        function callPetDetails(model){
            petDetails.init(model, mainContainer, initialize);
        }

        // call pet edit presenter, which shows pet edit view
        function callPetEdit(model){
            petEdit.init(model, mainContainer, initialize);
        }

        // call pet create presenter, which shows pet create view
        function callPetCreate(){
            petCreate.init(mainContainer, initialize);
        }

        function initialize(viewContainer){
            console.log("init pets owned presenter");
            mainContainer = viewContainer || mainContainer || $("body");
            renderView();
            setDataTable();
            attachHandlers();  
        }            

        return {
            init: function(viewContainer){
                $(document).ready(function () {
                    initialize(viewContainer);
                });
            }
        }

    }
);



// var jqXHR = $.ajax({
//   url: "target.aspx",
//   type: "GET",
//   dataType: "html",
// }).done(function (data, status, jqXHR) {
//   $("#container").html(data);
//   alert("Promise success callback.");
// }).fail(function (jqXHR,status,err) {
//   alert("Promise error callback.");
// }).always(function () {
//   alert("Promise completion callback.");
// })