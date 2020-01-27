function SaveUser() {
    var name = $("#fname").val();
    var email = $("#lname").val();
    var ne = $("#country").val();

    $.ajax({
        type: "post",
        url: 'AddUser',
        data: JSON.stringify({ Name: name, Email: email, NE: ne }),
      contentType: "application/json",

        success: function () {
            $("#titre").html("<strong>Please verify your Email </strong>");
            $("#formulr").html("you will receive shortly an Email with your credentials");
        }
    })

}

function AddChapters() {
    var test = $("#chap").val();
    element = document.getElementById("div1");
    element.innerHTML="";
    for (var i = 0; i < test; i++) {
        var lbl = document.createElement("label");
        var textnode = document.createTextNode("chapter");
        lbl.appendChild(textnode);
        element.appendChild(lbl);
        var input = document.createElement("input");
        input.setAttribute("type", "text");
        input.setAttribute("name", "chap" + i.toString());
        input.setAttribute("id", "chap" + i.toString());
        input.setAttribute("placeholder", "Chapter name");
        element.appendChild(input);

    }
}

function SaveCours() {
    var title = $("#title").val();
    var education = $("#education").val();
    var numb = $("#chap").val();
    var date = $("#date").val();
    var description = $("#description").val();
    var image = $("#image").val();
    var chap = "";
    for (var i = 0; i < numb; i++) {
        var content = $("#chap" + i.toString()).val();
        chap += content+",";
        
    }
    $.ajax({
        type: "post",
        url: 'AddCours',
        data: JSON.stringify({ Title: title, Education: education, Chapters: chap, Date:date,Description:description, Image:image}),
        contentType: "application/json",

        success: function () {
          //  $("#titre").html("<strong>Please verify your Email </strong>");
            $("#formulr").html("Course Added successfully");
        }
    })

}