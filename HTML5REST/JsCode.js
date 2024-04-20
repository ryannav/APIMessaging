
function sendMsg() {                       //function to call the api
   var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (xhttp.readyState == 4 && xhttp.status == 200) {
            yourNum = xhttp.responseText;
            document.getElementById("serviceCallValue").innerText = "Message has been sent";
        } else {
            document.getElementById("serviceCallValue").innerText = "Error, message has not been sent";
        }
    }
    
    var receiver = document.getElementById("Text1").value; //gets sender information
    var sender = document.getElementById("Text2").value;
    var msg = document.getElementById("Text3").value;

    var uri = "http://localhost:8080/Service1.svc/SendMsg?receiver="+receiver+"&sender="+sender+"&msg="+ msg; //generate URI for the API call, adjust localhost number if needed
    
    xhttp.open("GET", uri, true);
    xhttp.setRequestHeader("Content-type", "application/xml");
    xhttp.send();
}

function getMsg() {                       //function to call the api
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (xhttp.readyState == 4 && xhttp.status == 200) {
            var text = xhttp.responseText;
            var parser = new DOMParser();
            var xmlDoc = parser.parseFromString(text, "text/xml");
            var stringNodes = xmlDoc.getElementsByTagName("string");
            var output = "";

            for (var i = 0; i < stringNodes.length; i += 3) {       //function to parse the XML
                var senderID = stringNodes[i].textContent;
                var timeSent = stringNodes[i + 1].textContent;
                var messageContents = stringNodes[i + 2].textContent;

                output += "\nSender: " + senderID;              //concatonates string
                output += "\nTime Sent: " + timeSent;
                output += "\nMessage: " + messageContents;
                output += "\n";
            }
            document.getElementById("allMessages").innerText = output;
            if (output.length == 0) {
                document.getElementById("allMessages").innerText = "You have no current messages";
            }
        } else {
            document.getElementById("allMessages").innerText = "There was an error";
        }
    }

    var id = document.getElementById("idBox").value; //gets the upper and loser bound
    var checkbox = document.getElementById("purge");
    var boolValue = checkbox.checked && checkbox.value === "yes";

    var uri = "http://localhost:8080/Service1.svc/ReceiveMsg?receiver="+id+"&purge="+boolValue; //generate URI for the API call, adjust localhost number if needed

    xhttp.open("GET", uri, true);
    xhttp.setRequestHeader("Content-type", "application/xml");
    xhttp.send();
}