
var messageBuilder = function () {
    var message = null;
    var inside = null;
    var container = null;
    var header = null;
    var content = null;

    return {
        createMessage: function (classList,offsetClass,containerClasses) {
            message = document.createElement("div")
            if (classList === undefined)
                classList = [];

            for (var i = 0; i < classList.length; i++) {
                message.classList.add(classList[i])
            }

            inside = document.createElement("div")
            if (offsetClass != undefined) inside.classList.add(offsetClass);

            message.appendChild(inside);

            var br = document.createElement("br");

            inside.appendChild(br);

            if (containerClasses === undefined) containerClasses = [];
            container = document.createElement("div");
            for (var i = 0; i < containerClasses.length; i++) {
                container.classList.add(containerClasses[i])
            }

            inside.appendChild(container);

            return this;
        },
        withHeader: function (text) {
            header = document.createElement("div")
            header.classList.add('font-weight-bold')
            header.classList.add('text-left')

           

            header.appendChild(document.createTextNode(text))
            return this;
        },
        withContent: function (text,classes) {
            content = document.createElement("div")
            if (classes === undefined) classes = [];
            content.classList.add('p-2')
            content.classList.add('ml-2')
            content.style.maxWidth = '400px'
            content.style.overflowWrap = 'break-word';

            for (var i = 0; i < classes.length; i++) {
                content.classList.add(classes[i])
            }

            content.appendChild(document.createTextNode(text))
            return this;
        },
       
        build: function () {
            container.appendChild(header);
            container.appendChild(content);
            
            return message;
        }
    }
}