
$("#frm-register").validate({
    rules: {
        Password: {
            function(value, element) {
                return this.optional(element) || /[a-z]+@[a-z]+\.[a-z]+/.test(value);
            },
            minlength: 6
        }
    },
    messages: {
        Password: "Please enter Strong Password",        
    },
    submitHandler: function (form) {
        form.submit();
    }
});