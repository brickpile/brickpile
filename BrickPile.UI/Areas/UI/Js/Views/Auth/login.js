define([
        'jquery',
        'underscore',
        'backbone',
        'views/base',
        'models/user'
    ], function($, _, Backbone, BaseView, User) {

        var LoginView = BaseView.extend({
            template: _.template($('#view-template-login-view').html()),

            events: {
                'click button': 'login',
            },

            initialize: function() {

            },

            render: function() {
                var html = this.template(this.model.toJSON());
                $(this.el).html(html);
                return this;
            },

            login: function(event) {

                var self = this;

                var app = brickpile.app;

                event.preventDefault();

                self.$el.find('form').removeClass('animated fadeInDown');

                this.username = this.$el.find('#UserName');
                this.password = this.$el.find('#Password');
                this.confirmPassword = this.$el.find('#ConfirmPassword');

                var user = new User({ username: this.username.val(), password: this.password.val() });

                var url = '/api/auth/login';

                $.ajax({
                    url: url,
                    type: 'POST',
                    data: JSON.stringify(user),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function() {

                        setTimeout(function() {
                            $('#login-screen').addClass('hidden');
                            app.router.navigate('ui/', true);
                        }, 500);
                        setTimeout(function() {
                            $('#login-screen').css({ display: 'none' });
                            self.dispose();
                        }, 1000);

                    },
                    error: function(jqXhr) {
                        self.$el.find('form').addClass('error');
                        setTimeout(function() {
                            self.$el.find('form').removeClass('error');
                        }, 1000);
                    }
                });
            }
        });
        return LoginView;
    });