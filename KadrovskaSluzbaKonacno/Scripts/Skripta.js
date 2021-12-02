$(document).ready(function () {

    var host = window.location.host;
    var token = null;
    var headers = {};
    var jediniceEndpoint = "/api/jedinice/";
    var zaposleniEndpoint = "/api/zaposleni/";

    loadZaposleni();

    function loadZaposleni() {
        var requestUrl = "https://" + host + zaposleniEndpoint;
        $.getJSON(requestUrl, setZaposleni);
    }

    function setZaposleni(data, status) {

        var container = $("#dataZaposleni");
        container.empty();

        if (status == "success") {

            var div = $("<div style='text-align: center'></div>");
            var h3 = $("<h3><b>Zaposleni</b></h3>");

            div.append(h3);

            var table = $("<table></table>");

            if (token) {
                var header = $("<thead><td>Ime i prezime</td><td>Rola</td><td>Godina zaposlenja</td><td>Godina rodjenja</td><td>Jedinica</td><td>Plata</td><td>Akcija</td></thead>");
            } else {
                header = $("<thead><td>Ime i prezime</td><td>Rola</td><td>Godina zaposlenja</td><td>Jedinica</td></thead>");
            }

            table.append(header);

            for (i = 0; i < data.length; i++) {

                var row = "<tr>";

                if (token) {

                    var displayData = "<td>" + data[i].ImeIPrezime + "</td><td>" + data[i].Rola + "</td><td>" + data[i].GodinaZaposlenja + "</td><td>" + data[i].GodinaRodjenja + "</td><td>" + data[i].Jedinica.Ime + "</td><td>" + data[i].Plata + "</td>";

                }

                else {

                    displayData = "<td>" + data[i].ImeIPrezime + "</td><td>" + data[i].Rola + "</td><td>" + data[i].GodinaZaposlenja + "</td><td>" + data[i].Jedinica.Ime + "</td>"
                }

                var stringId = data[i].Id.toString();

                var displayDelete = "<td><button id=btnDelete name=" + stringId + ">[Obrisi]</button></td>";

                if (token) {
                    row += displayData + displayDelete + "</tr>";
                } else {
                    row += displayData + "</tr>";
                }

                table.append(row);
            }

            div.append(table);

            if (token) {

                var requestUrlMesta = "https://" + host + jediniceEndpoint;
                $.getJSON(requestUrlMesta, setDropDownList)

                $("#formPretragaDiv").css("display", "block");
                $("#formZaposleniDiv").css("display", "block");
            }

            container.append(div);
        }
        else {
            alert("Greska prilikom dobavljanja Zaposlenih!");
        }
    }

    function setDropDownList(data, status) {
        var container = $("#zaposlenJedinica");
        container.empty();

        if (status == "success") {
            for (i = 0; i < data.length; i++) {
                var option = "<option value=" + data[i].Id + ">" + data[i].Ime + "</option>";
                container.append(option);
            }
            $("#zaposlenJedinica").val('');
        }
        else {
            alert("Desila se greška prilikom učitavanja Jedinica u padajući meni!");
        }
    }

    $("#btnRegistracija").click(function () {
        $("#pocetakInfo").css("display", "none");
        $("#pocetakButtons").css("display", "none");
        $("#registracijaDiv").css("display", "block");
    });

    $("#registracija").submit(function (e) {
        e.preventDefault();

        var email = $("#regEmail").val();
        var loz = $("#regLoz").val();

        var sendData = {
            "Email": email,
            "Password": loz,
            "ConfirmPassword": loz
        };

        $.ajax({
            type: "POST",
            url: 'https://' + host + "/api/Account/Register",
            data: sendData

        }).done(function (data, status) {
            $("#regEmail").val('');
            $("#regLoz").val('');
            $("#pocetakInfo").css("display", "none");
            $("#pocetakButtons").css("display", "none");
            $("#registracijaDiv").css("display", "none");
            $("#prijavaDiv").css("display", "block");

        }).fail(function (data, status) {
            alert("Greska prilikom registracije!");
        });
    });

    $("#btnOdustajanjeRegistracija").click(function () {
        $("#registracijaDiv").css("display", "none");
        $("#pocetakInfo").css("display", "block");
        $("#pocetakButtons").css("display", "block");
    });

    $("#btnPrijava").click(function () {
        $("#pocetakInfo").css("display", "none");
        $("#pocetakButtons").css("display", "none");
        $("#prijavaDiv").css("display", "block");
    })

    $("#prijava").submit(function (e) {
        e.preventDefault();

        var email = $("#priEmail").val();
        var loz = $("#priLoz").val();

        var sendData = {
            "grant_type": "password",
            "username": email,
            "password": loz
        };

        $.ajax({
            type: "POST",
            url: "https://" + host + "/Token",
            data: sendData

        }).done(function (data, status) {
            console.log(data);
            $("#info").empty().append("Prijavljen korisnik: " + data.userName);
            token = data.access_token;
            $("#priEmail").val('');
            $("#priLoz").val('');
            $("#pocetakInfo").css("display", "none");
            $("#pocetakButtons").css("display", "none");
            $("#prijavaDiv").css("display", "none");
            $("#prijavljenDiv").css("display", "block");
            loadZaposleni();
        }).fail(function (data, status) {
            alert("Greska prilikom prijave!");
        });
    });

    $("#btnOdustajanjePrijava").click(function () {
        $("#prijavaDiv").css("display", "none");
        $("#pocetakInfo").css("display", "block");
        $("#pocetakButtons").css("display", "block");
    });

    $("#odjavise").click(function () {

        token = null;
        headers = {};

        $("#prijavljenDiv").css("display", "none");
        $("#formPretragaDiv").css("display", "none");
        $("#formZaposleniDiv").css("display", "none");
        $("#pocetakInfo").css("display", "block");
        $("#pocetakButtons").css("display", "block");
        loadZaposleni();
    });

    $("body").on("click", "#btnDelete", deleteZaposlen);

    function deleteZaposlen() {

        var deleteID = this.name;

        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            url: 'https://' + host + zaposleniEndpoint + deleteID.toString(),
            type: "DELETE",
            headers: headers
        })
            .done(function (data, status) {
                loadZaposleni();
            })
            .fail(function (data, status) {
                alert("Desila se greška prilikom brisanja Zaposlenog!");
            });

    }

    $("#zaposleniPretragaForm").submit(function (e) {

        e.preventDefault();

        var najmanje = $("#najmanje").val();
        var najvise = $("#najvise").val();

        var sendData = {
            "Najmanje": najmanje,
            "Najvise": najvise
        };

        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            url: 'https://' + host + "/api/pretraga",
            type: "POST",
            data: sendData,
            headers: headers
        }).done(function (data, status) {
            setZaposleniPretraga(data, status);
        }).fail(function (data, status) {
            alert("Greška prilikom submitovanja forme za pretragu zaposlenih!");
        })
    });

    function setZaposleniPretraga(data, status) {

        var container = $("#dataZaposleni");
        container.empty();

        if (status == "success") {

            var div = $("<div style='text-align: center'></div>");
            var h3 = $("<h3><b>Zaposleni</b></h3>");

            div.append(h3);

            var table = $("<table></table>");

            if (token) {
                var header = $("<thead><td>Ime i prezime</td><td>Rola</td><td>Godina zaposlenja</td><td>Godina rodjenja</td><td>Jedinica</td><td>Plata</td><td>Akcija</td></thead>");
            } else {
                header = $("<thead><td>Ime i prezime</td><td>Rola</td><td>Godina zaposlenja</td><td>Jedinica</td></thead>");
            }

            table.append(header);

            for (i = 0; i < data.length; i++) {

                var row = "<tr>";

                if (token) {

                    var displayData = "<td>" + data[i].ImeIPrezime + "</td><td>" + data[i].Rola + "</td><td>" + data[i].GodinaZaposlenja + "</td><td>" + data[i].GodinaRodjenja + "</td><td>" + data[i].Jedinica.Ime + "</td><td>" + data[i].Plata + "</td>";

                }

                else {

                    displayData = "<td>" + data[i].ImeIPrezime + "</td><td>" + data[i].Rola + "</td><td>" + data[i].GodinaZaposlenja + "</td><td>" + data[i].Jedinica.Ime + "</td>"
                }

                var stringId = data[i].Id.toString();

                var displayDelete = "<td><button id=btnDelete name=" + stringId + ">[Obrisi]</button></td>";

                if (token) {
                    row += displayData + displayDelete + "</tr>";
                } else {
                    row += displayData + "</tr>";
                }

                table.append(row);
            }

            div.append(table);

            container.append(div);
        }
        else {
            alert("Greška prilikom dobavljanja Zaposlenih!");
        }
    }

    $("#zaposleniForm").submit(function (e) {

        e.preventDefault();

        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var jedinica = $("#zaposlenJedinica").val();
        var rola = $("#zaposlenRola").val();
        var imeIPrezime = $("#zaposlenImeIPrezime").val();
        var godinaRodjenja = $("#zaposlenGodinaRodjenja").val();
        var godinaZaposlenja = $("#zaposlenGodinaZaposlenja").val();
        var plata = $("#zaposlenPlata").val();

        var sendData = {
            "ImeIPrezime": imeIPrezime,
            "Rola": rola,
            "GodinaRodjenja": godinaRodjenja,
            "GodinaZaposlenja": godinaZaposlenja,
            "Plata": plata,
            "JedinicaId": jedinica
        };

        console.log(sendData);

        $.ajax({
            url: "https://" + host + zaposleniEndpoint,
            type: "POST",
            data: sendData,
            headers: headers
        }).done(function (data, status) {
            $("#zaposlenJedinica").val('');
            $("#zaposlenRola").val('');
            $("#zaposlenImeIPrezime").val('');
            $("#zaposlenGodinaRodjenja").val('');
            $("#zaposlenGodinaZaposlenja").val('');
            $("#zaposlenPlata").val('');
            loadZaposleni();
        }).fail(function (data, status) {
            alert("Desila se greška prilikom dodavanja Zaposlenog!");
        });
    });

    $("#btnOdustajanjeForm").click(function () {
        $("#zaposlenJedinica").val('');
        $("#zaposlenRola").val('');
        $("#zaposlenImeIPrezime").val('');
        $("#zaposlenGodinaRodjenja").val('');
        $("#zaposlenGodinaZaposlenja").val('');
        $("#zaposlenPlata").val('');
    });
});