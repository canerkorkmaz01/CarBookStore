
  $(document).ready(function () {
            var enable = $("#Enabled").val();

      if ($('#Enabled').is(':checked')) {
    $("#check").text("Aktif");
            }
  else {
    $("#check").text("Pasif");
            }
  $("input[type='checkbox']").click(function () {
                
                if (this.checked) {
    $("#check").text("Aktif");
                }
  else {
    $("#check").text("Pasif");
                }
            });
          
        });

