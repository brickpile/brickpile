var LoginView = BaseView.extend({

    el: $("#login-form"),

    events: {
        'click button': 'login',
    },

    initialize: function () {
        
    },

    login: function (event) {
        
        var self = this;

        var app = brickpile.app;
        
        event.preventDefault();

        self.$el.removeClass('animated fadeInDown');

        var url = '/api/auth/login';

        $.ajax({
            url: url,
            type: 'POST',
            data: this.$el.serialize(),
            success: function () {

                self.dispose();

                setTimeout(function () {
                    $('#login-screen').addClass('hidden');
                    app.router.navigate('ui/', true);
                }, 500);
                setTimeout(function () {
                    $('#login-screen').css({ display: 'none' });
                }, 1000);
            },
            error: function (jqXhr) {
                self.$el.addClass('error');
                setTimeout(function () {
                    self.$el.removeClass('error');
                }, 1000);
            }
        });
    }   
});