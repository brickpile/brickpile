define([
        'jquery',
        'underscore',
        'backbone'
    ], function($, _, Backbone) {
        var Page = Backbone.Model.extend({
            defaults: {
                metadata: {
                    name: null,
                    title: null,
                    keywords: null,
                    description: null,
                    displayInMenu: null,
                    published: null,
                    changed: null,
                    changedBy: null,
                    isPublished: null,
                    isDeleted: null,
                    slug: null,
                    url: null,
                    sortOrder: null
                },
                parent: null
            },
            url: function() {
                if (this.id) {
                    return '/api/page/' + this.id;
                }
                return '/api/page/';
            },
            initialize: function() {
            },
            toJSON: function() {
                // build the "$type" as the first parameter
                var json = { '$type': this.get('$type') };
                // get the rest of the data
                _.extend(json, Backbone.Model.prototype.toJSON.call(this));
                // send it back, and hope it's in the right order
                return json;
            }
        });

        return Page;
    });
