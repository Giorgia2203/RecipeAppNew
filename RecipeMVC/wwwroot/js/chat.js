"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g,
        "&gt;");
    var li = document.createElement("li");

    var divContainer = document.createElement("div");
    divContainer.className = "card border-secondary mb-3";
    divContainer.style.maxWidth = "20rem";

    var divHeader = document.createElement("div");
    divHeader.className = "card-header text-white bg-secondary";
    divHeader.style.fontWeight = "bold";
    divHeader.innerHTML = user;

    var divBody = document.createElement("div");
    divBody.className = "card-body";

    var pContent = document.createElement("p");
    pContent.innerHTML = msg;

    divBody.appendChild(pContent);
    divContainer.appendChild(divHeader);
    divContainer.appendChild(divBody);

    console.log(divContainer)
    li.appendChild(divContainer);
    console.log(li)
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", "", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});