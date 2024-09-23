document.addEventListener("DOMContentLoaded", () => {

    let connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    connection.start().then(function () {
        console.log("Connected to SignalR hub as: " + connection.connectionId);
    }).catch(function (err) {
        return console.error(err.toString());
    });

    const form = document.querySelector("form");

    form.addEventListener("submit", function (event) {
        event.preventDefault();

        const userId = parseInt(document.querySelector('input[name="userId"]').value, 10);
        const orderId = parseInt(document.querySelector('select[name="orderId"]').value, 10);
        const connectionId = connection.connectionId

        let href = '/CreateChat'; 
        fetch(href, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                userId: userId, 
                orderId: orderId, 
                connectionId: connectionId
            })
        })
            .then(response => {
                if (!response.ok) {
                    console.log("Ошибка в ответе сервера:", response.statusText);
                    throw new Error('Network response was not ok');
                }
                return console.log("Выполнено успешно !!!")
            })
            .then(data => {
                console.log('Chat created:', data);
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    });
})