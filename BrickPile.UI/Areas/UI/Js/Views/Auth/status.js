var LoginStatusView = Backbone.View.extend({
    
    events: {
        'click button': 'logoff'
    },
    initialize: function () {
        
    },
    template: _.template($('#view-template-login-status-view').html()),
    render: function () {
        $(this.el).html(this.template(this.model.toJSON()));
        return this;
    },
    logoff: function () {
        var app = brickpile.app;
        var url = '/api/auth/logoff';
        $.ajax({
            url: url,
            type: 'POST',
            success: function () {
                $('#login-screen').removeClass('hidden').removeAttr('style');
                $('#login-screen').addClass('animated fadeInDown');
                app.router.navigate('ui/login/', true);
            }
        });
    }
});