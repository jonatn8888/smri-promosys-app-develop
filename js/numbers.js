// -------------------------------------------------------------------
// numbers.js
//      - Functions for numeric format
//
//
// Noel B. Santos
// -------------------------------------------------------------------


// decimal only
function AllowDecimalOnly(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode;
    
    if (charCode != 46 && charCode > 31 
        && (charCode < 48 || charCode > 57))
         return false;
    
    return true;
}

// integer only

function AllowIntegerOnly(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode;
    
    if (charCode > 31 && (charCode < 48 || charCode > 57)) return false;
    
    return true;
}

function addCommas(nStr)
{
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
	    x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

// This function removes non-numeric characters

function stripNonNumeric( str )
{
  str += '';
  var rgx = /^\d|\.|-$/;
  var out = '';
  for( var i = 0; i < str.length; i++ )
  {
    if( rgx.test( str.charAt(i) ) ){
      if( !( ( str.charAt(i) == '.' && out.indexOf( '.' ) != -1 ) ||
             ( str.charAt(i) == '-' && out.length != 0 ) ) ){
        out += str.charAt(i);
      }
    }
  }
  return out;
}

// removal of non-numeric formatting (used for onfocus)
function PrepNumericTextbox( oTextBox, nIntOnly )
{
    var nValue = Number(stripNonNumeric(oTextBox.value));
       
    if (nIntOnly) oTextBox.value = nValue.toFixed(0);
    else oTextBox.value = nValue;
    
    oTextBox.value = stripNonNumeric(oTextBox.value);
    
    oTextBox.select();
}


function AllowNumericOnly(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode
    
    if (charCode > 31 && (charCode < 48 || charCode > 57)) return false;

    return true;
}

/********************************
    function AllowNumericOnly(e)
    {
        var keycode;
        if (window.event) keycode = window.event.keyCode;
        else if (event) keycode = event.keyCode;
        else if (e) keycode = e.which;
        else return true;
        
        if( (keycode > 47 && keycode <= 57) )
            {
            return true;
            }
        else {
            return false;
            }
        return true;
    }
*********************************/

function FormatNumericTextbox(oTextBox, nNumDecimals)
{
    // format displayed values
    // document.getElementById('<%= txtPlanPromoSales.ClientID %>').value = addCommas(nPlanPromoSales.toFixed(2));

    //if (nIntOnly) oTextBox.value = nValue.toFixed(0);
    //else oTextBox.value = nValue;

}
