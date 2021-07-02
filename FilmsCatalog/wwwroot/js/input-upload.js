document.querySelector('.custom-file-input').addEventListener('change', function (e) {
    debugger;
    var fileName = document.getElementById("myInput").files[0].name;
    var nextSibling = e.target.nextElementSibling
    nextSibling.innerText = fileName
})