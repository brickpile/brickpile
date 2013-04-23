var RegisterView = BaseView.extend({

    template: _.template($('#view-template-setup-view').html()),

    events: {
        'click button': 'register'
    },

    initialize: function () { },

    render: function () {
        var html = this.template();
        $(this.el).html(html);
        return this;
    },

    register: function (event) {

        var self = this;

        event.preventDefault(); // Don't let this button submit the form

        self.$el.find('form').removeClass('animated fadeInDown');

        this.username = this.$el.find('#UserName');
        this.password = this.$el.find('#Password');
        this.confirmPassword = this.$el.find('#ConfirmPassword');

        var userModel = new RegisterModel({ username: this.username.val(), password: this.password.val(), confirmPassword: this.confirmPassword.val() });

        var url = '/api/auth/';

        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify(userModel),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function () {
                setTimeout(function () {
                    $('#login-screen').addClass('hidden');
                    app.router.navigate('ui/', true);
                }, 500);
                setTimeout(function () {
                    $('#login-screen').css({ display: 'none' });
                    self.dispose();
                }, 1000);
            },
            error: function (jqXhr) {
                self.$el.find('form').addClass('error');
                setTimeout(function () {
                    self.$el.find('form').removeClass('error');
                }, 1000);

            }
        });
    }
});