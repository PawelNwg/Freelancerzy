
const deleteMessage = (id) => {

    axios.delete('/Chat/Delete', {
        params: {
            id: id
        }
    })
        .then(res => {

        })
        .catch(err => {
            console.log("Failed to delete message!")
        })
}

const messageBuilder = function () {
    let message = null;
    let inside = null;
    let container = null;
    let header = null;
    let content = null;
    let deletebutton = null;

    return {
        createMessage: function (classList, offsetClass, containerClasses,id) {
            message = document.createElement("div")
            if (classList === undefined)
                classList = [];

            for (var i = 0; i < classList.length; i++) {
                message.classList.add(classList[i])
            }

            inside = document.createElement("div")
            if (offsetClass != undefined) inside.classList.add(offsetClass);

            message.appendChild(inside);

            const br = document.createElement("br");

            inside.appendChild(br);

            if (containerClasses === undefined) containerClasses = [];
            container = document.createElement("div");
            for (var i = 0; i < containerClasses.length; i++) {
                container.classList.add(containerClasses[i])
            }

            container.id = "message+" +id

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
        withDeleteButton: function (id, deletable) {
            if (deletable) {
                deletebutton = document.createElement("button")
                deletebutton.classList.add("btn")
                deletebutton.classList.add("btn-outline-info")
                deletebutton.classList.add("deleteBtn")
                deletebutton.innerHTML = "Usuń"
                deletebutton.addEventListener("click", () => { deleteMessage(id) });
                header.appendChild(deletebutton)
            }
            return this;
        },
        withContent: function (text, classes) {
            content = document.createElement("div")
            if (classes === undefined) classes = [];
            content.classList.add('p-2')
            content.classList.add('ml-2')
            content.classList.add('content')
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




var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();
var _connectionId = '';

connection.on("DeleteMessage",
    function (data) {
        const container = document.getElementById(`message+${data.id}`)
        const content = container.getElementsByClassName("content")[0]
        content.innerHTML = "Wiadomość została usunięta przez użytkownika"

        const button = container.getElementsByClassName("deleteBtn")[0]
        button.style.display = "none"

    })

connection.on("RecieveMessage",
    function (data) {

        var date = new Date(data.date).toLocaleString();
        var classList = [];
        var offsetClass = '';
        var containerClasses = [];
        var contentClasses = [];
        var deletable = false;

        if (data.userName == userName) {
            classList = ['row', 'm-2', 'd-flex', 'flex-row-reverse']
            offsetClass = 'flex-row-reverse';
            containerClasses = ['container', 'primary']
            contentClasses = ['text-left', 'bg-primary'];
            deletable = true;
        }
        else {
            classList = ['row', 'm-2', 'd-flex', 'flex-row']
            offsetClass = 'flex-row';
            containerClasses = ['container']
            contentClasses = ['text-left', 'bg-light'];
        }

        

        var message = messageBuilder()
            .createMessage(classList, offsetClass, containerClasses,data.id)
            .withHeader(date)
            .withDeleteButton(data.id, deletable)
            .withContent(data.text, contentClasses)
            .build();
        document.querySelector('.chat-body').append(message);
        document.getElementById('chat').scrollTo(0, document.getElementById('chat').scrollHeight);
    })
connection.start()
    .then(function () {
        connection.invoke('joinRoom', `${connectionID}`);
    })
    .catch(function (err) {
        console.log(err)
    })

window.addEventListener('onunload', function () {
    connection.invoke('leaveRoom', `${connectionID}`);
})
var sendMessage = function (event) {
    event.preventDefault();
    var data = new FormData(event.target);
    document.getElementById('message-input').value = '';
    axios.post('/Chat/Create', data)
        .then(res => {

        })
        .catch(err => {
            console.log("Failed to send message!")
        })
}