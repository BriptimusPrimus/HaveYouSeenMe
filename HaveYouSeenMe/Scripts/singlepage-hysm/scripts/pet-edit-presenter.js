define(["view-renderer", "pet-details-presenter"],
	function(renderer, petDetails){
		var model,
			view = "pet-edit-view",
			mainContainer,
			fnGoBack = function(){};

		function renderView(){
			renderer.renderTemplate(view, mainContainer, model);		
		}

		function attachHandlers(){
			$('#back-to-list').on( 'click', function (e) {
				e.preventDefault();		
				//invoke the callback to render previous view			
				fnGoBack();
			});

			$("#btn-save").on('click', function(e){
				e.preventDefault();
				getFormValues();
				if(validatePetData(model)){
					console.log("updating pet...")
					savePet(model);
				}
			});

            //trigger native file select dialog
            $("#PetImage").on("click", function (e) {
                e.preventDefault();
                $("#PictureFile").click();                
            });

            $("#PictureFile").on("change", function (e) {
                if (this.value != "") {
                	console.log("value "+this.value);
                	console.log(e.target.files);
                    uploadFile(e.target.files);
                }
            });			
		}

		function uploadFile(files){

            var formData= new FormData();
            // var imageFile = document.getElementById("PictureFile").files[0];
            var imageFile = files[0];                        		    
			var pictureModel = {
				PictureFile : "",
				PetID : model.PetID,
				PetName : model.PetName
			};
			formData.append("model", imageFile);
			formData.append("petId", model.PetID);
			formData.append("petName", model.PetName);

			var jqXHR = $.ajax({
			  	url: "PictureUploadAjaxHandler",
			  	type: "POST",
			  	data: formData,
				cache: false,
		        dataType: 'json',
		        processData: false, // Don't process the files
		        // Set content type to false as jQuery will tell the server its a query string request
		        contentType: false, 
			}).done(function (data, status, jqXHR) {			  	
			  	if(data.message){
				  	//go to pet details
				  	petDetails.init(model, mainContainer, fnGoBack);
			  	} else {
					alert("Error: " + data.error);
			  	}
			}).fail(function (jqXHR,status,err) {
				alert("Could not upload picture. " + err);
				console.log(err);
				console.log(jqHXR);
			}).always(function () {
			})			
		}

		//set the input from the form as model values
		function getFormValues(){
			model.PetAgeYears = $("#PetAgeYears").val();
			model.PetAgeMonths = $("#PetAgeMonths").val();
			model.StatusID = $("#StatusID").val();
			model.LastSeenOn = $("#LastSeenOn").val();
			model.LastSeenWhere = $("#LastSeenWhere").val();
			model.Notes = $("#Notes").val();
		}

		function savePet(pet){
			var jqXHR = $.ajax({
			  	url: "EditAjaxHandler",
			  	type: "POST",
			  	data: pet,
			  	dataType: "json"
			}).done(function (data, status, jqXHR) {
			  	if(data.message){
			  		//go back to pets owned list
			  		fnGoBack();
			  	} else {
			  		alert("Error: " + data.error);			  		
			  	}
			}).fail(function (jqXHR,status,err) {
				alert("Could not update pet. " + err);
			}).always(function () {
			});			
		}

		function validatePetData(pet){
			//find any elements marked with ASP .NET MVC error classes
			if ($(".input-validation-error").length
				|| $(".field-validation-error").length){
					return	false;
			}

			//no custom validations this time ... only the ones in the view model

			return true;
		}

		function initialize(pet, viewContainer, callback){
			model = pet;
			//assign container and callback if passed
			mainContainer = viewContainer || mainContainer || $("body");
			fnGoBack = callback || fnGoBack;			
			renderView();
			attachHandlers();
		}		

		return{
			init: function(pet, viewContainer, callback){
				console.log("init pet edit presenter");
				initialize(pet, viewContainer, callback);		
			}
		}
	}
);