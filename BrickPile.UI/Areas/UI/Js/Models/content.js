var Content = Backbone.Model.extend({
    url: function() {
        if (this.id) {
            return '/api/content/' + this.id.replace('/','-');
        }
        return '/api/content/';
    },
    toJSON: function(){
        // build the "$type" as the first parameter
        var json = { '$type': this.get('$type')};
        // get the rest of the data
        _.extend(json, Backbone.Model.prototype.toJSON.call(this));
        // send it back, and hope it's in the right order
        return json;
    }
});