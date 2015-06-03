define(["view-renderer", "pet-edit-presenter"],
	function(renderer, petEdit){
		var view = "pet-create-view",
			mainContainer,
			fnGoBack = function(){};

		function renderView(){
			renderer.renderTemplate(view, mainContainer);		
		}

		function attachHandlers(){
			$('#back-to-list').on( 'click', function (e) {
				e.preventDefault();		
				//invoke the callback to render previous view			
				fnGoBack();
			});

			$("#btn-save").on('click', function(e){
				e.preventDefault();
				var model = getFormValues();
				if(validatePetData(model)){
					console.log("saving pet...")
					savePet(model);
				}
			});			
		}

		//set the input from the form as model values
		function getFormValues(){
			var model = {};
			model.PetName = $("#PetName").val();
			model.PetAgeYears = $("#PetAgeYears").val();
			model.PetAgeMonths = $("#PetAgeMonths").val();
			model.StatusID = 1; // initial state is lost
			model.LastSeenOn = $("#LastSeenOn").val();
			model.LastSeenWhere = $("#LastSeenWhere").val();
			model.Notes = $("#Notes").val();
			return model;
		}

		function savePet(pet){
			var jqXHR = $.ajax({
			  	url: "CreateAjaxHandler",
			  	type: "POST",
			  	data: pet,
			  	dataType: "json"
			}).done(function (data, status, jqXHR) {
			  	if(data.model){
				  	//go edit pet screen
				  	console.log(data); console.log(data.model);
				  	petEdit.init(data.model, mainContainer, fnGoBack);
			  	} else {
			  		alert("Error: " + data.error);
			  	}
			}).fail(function (jqXHR,status,err) {
				alert("Could not save pet. " + err);
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

		function initialize(viewContainer, callback){
			//assign container and callback if passed
			mainContainer = viewContainer || mainContainer || $("body");
			fnGoBack = callback || fnGoBack;			
			renderView();
			attachHandlers();
		}

		return{
			init: function(pet, viewContainer, callback){
				console.log("init pet create presenter");
				initialize(pet, viewContainer, callback);		
			}
		}		

	}		
);