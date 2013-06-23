var ContentEditView = Backbone.View.extend({
    events: {
        'click input[type=submit]': 'save'
    },
    initialize: function () {
        try {
            var type = this.model.get('$type');
            var index = type.indexOf(',');
            this.template = _.template($('#edit-template-' + type.substring(0, index).replace(/\./g, '-').toLowerCase()).html());
        } catch (e) {
            console.log('Unable to set editor template for model ' + e);
        }
    },
    render: function () {
        try {
            var html = this.template(this.model.toJSON());
            $(this.el).html(html);

        } catch (err) {
            var txt = "There was an error on this page.\n\n";
            txt += "Error description: " + err.message + "\n\n";
            txt += "Click OK to continue.\n\n";
            alert(txt);
        }
        return this;
    },
    save: function (e) {
        e.preventDefault();
        var formData = this.$el.find('form').toObject();
        this.model.set(formData);
        this.model.save(formData);
    }
});
