//Javascript print function
 function printContent(el){
        var restorepage = document.body.innerHTML;
    var printcontent = document.getElementById(el).innerHTML;
    document.body.innerHTML = printcontent;
    window.print();
    document.body.innerHTML = printcontent;
    history.go(0);
    }
