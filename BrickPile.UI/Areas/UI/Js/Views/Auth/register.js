var RegisterView = Backbone.View.extend({

    el: $("#register-form"),

    events: {
        'click button': 'register'
    },

    initialize: function () { },

    register: function (event) {

        var self = this;

        event.preventDefault(); // Don't let this button submit the form

        this.username = this.$el.find('#UserName');
        this.password = this.$el.find('#Password');
        this.confirmPassword = this.$el.find('#ConfirmPassword');

        var userModel = new RegisterModel({ username: this.username.val(), password: this.password.val(), confirmPassword: this.confirmPassword.val() });
        userModel.save(null, {
            success: function () {
                console.log('success');
                setTimeout(function () { $('#login-screen').addClass('hidden'); }, 500);
                setTimeout(function () { $('#login-screen').css({ display: 'none' }); }, 1000);
            },
            error: function (model, jqXHR) {
                if (jqXHR.status == 400) {
                    self.$el.addClass('error');
                    setTimeout(function () {
                        self.$el.removeClass('error');
                    }, 1000);
                }
            }
        });
    }
});