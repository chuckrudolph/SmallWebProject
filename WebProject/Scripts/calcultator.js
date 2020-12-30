/// <reference path="knockout-3.5.1.js" />


function AppViewModel() {

    let self = this;

    ko.validation.init({ insertMessages: false }); //To ensure that only custom error messages are displayed

    //Adding the math operation options to the dropdown menu
    self.mathOperation = ko.observableArray(["+", "-", "*", "/"]);

    //Adding observables and validation to the input values
    self.firstValue = ko.observable("0").extend({
        required: true,
        validation: {
            message: "Please enter a valid number",
            validator: function (value) {
                return !isNaN(value);
            }
        }
    });

    self.secondValue = ko.observable("0").extend({
        required: true,
        validation: {
            message: "Please enter a valid number",
            validator: function (value) {
                return !isNaN(value);
            }
        }
    });

    self.dropdownMenu = ko.observable();
    self.resultValue = ko.observable();
    self.dummyValue = ko.observable();

    //Calls the controller to calculate the result and it displays it into the result textbox
    self.getSolution = function () {
        //clear existing
        self.resultValue("");

        //It calls the Calculate method from the calculator controller and sends the two values and math symbol
        //to perform the required calculation. If successful, the result will be displayed in the result text box
        $.ajax({
            type: "POST",
            url: "/Calculator/Calculate",
            timeout: 6000,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                'firstNumber': self.firstValue(),
                'secondNumber': self.secondValue(),
                'mathOperation': self.dropdownMenu()
            }),
            dataType: "json",
            success: function (data) {
                //$("#result").val(data);
                self.resultValue(data);         //set result value
            },
            error: function () {
                alert("Failed at math operation");
            }
        });
    }

    //Subscribing the observables to the getSolution function
    self.firstValue.subscribe(self.getSolution);
    self.secondValue.subscribe(self.getSolution);
    self.dropdownMenu.subscribe(self.getSolution);
}


ko.applyBindings(new AppViewModel());