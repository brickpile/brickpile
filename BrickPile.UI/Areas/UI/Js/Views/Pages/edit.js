define([
        'jquery',
        'underscore',
        'backbone',
        'form2js',
        'form2jsToObject'
    ],
    function($, _, Backbone, Form2js, Form2jsToObject) {

        var PageEditView = Backbone.View.extend({
            events: {
                'click input[type=submit]': 'save'
            },
            initialize: function() {
                try {
                    var type = this.model.get('$type');
                    var index = type.indexOf(',');
                    this.template = _.template($('#edit-template-' + type.substring(0, index).replace(/\./g, '-').toLowerCase()).html());
                } catch (err) {
                    var txt = "There was an error on this page.\n\n";
                    txt += "Error description: Unable to set editor template for model " + err.message + "\n\n";
                    txt += "Click OK to continue.\n\n";
                    alert(txt);
                }
                //this.collection.bind("reset", this.render, this);
            },
            render: function() {

                try {
                    var html = this.template(this.model.toJSON());
                    $(this.el).html(html);
                } catch(err) {
                    var txt = "There was an error on this page.\n\n";
                    txt += "Error description: " + err.message + "\n\n";
                    txt += "Click OK to continue.\n\n";
                    alert(txt);
                }

                return this;
            },
            save: function(e) {
                e.preventDefault();
                var formData = $('#myForm').toObject();
                this.model.set(formData);
                this.model.save(formData);
            }
        });
        return PageEditView;
    });