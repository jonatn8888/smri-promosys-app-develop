

function checkAllSimple(chkAll,chkTagName,frm){
	var blnChecked=chkAll.checked;
	for(var i=0;i<frm.length;i++){
		e=frm.elements[i];
		if(e.type=='checkbox' && e.name.indexOf(chkTagName)!=-1){
		   if(e.disabled==false){
		    	e.checked=blnChecked;
		  	}
		}
	}
}

function changeCheckAll(chkAllId,chkTagName,frm){
	for(var i=0;i<frm.length;i++){
		e=frm.elements[i];
		if(e.type=='checkbox' && e.name.indexOf(chkTagName)!=-1){
			if (!e.checked && e.disabled==false){
				document.getElementById(chkAllId).checked=false;
				return;
			}
		}
	}
	document.getElementById(chkAllId).checked=true;
}

function changeCheckAllButton(chkAllId,chkTagName,btnTargetID,frm){
	for (var i=0;i<frm.length;i++){
			e=frm.elements[i];
			if(e.type=='checkbox' && e.name.indexOf(chkTagName)!=-1){
			  if (!e.checked && e.disabled==false){
				    document.getElementById(chkAllId).checked=false;
		
					return;
				}
							btnTargetID.disabled=false;
			}
		}
	document.getElementById(chkAllId).checked=true;
	btnTargetID.disabled=true;

}
function showControlByCheckbox(chkAllId,chkTag,chkTagName,id,hasSigns,frm){ 
    var blnChecked; 
    if (chkTag.checked && hasSigns>0) blnChecked="visible"; 
    else blnChecked="hidden"; 
    document.getElementById(id).style.visibility=blnChecked; 
    changeCheckAll(chkAllId,chkTagName,frm); 
}

function showControlByCheckbox2(chkTag,chkTagName,id,hasSigns,frm){ 
    
    var blnChecked; 
    
    if (chkTag.checked && hasSigns>0) 
    
        blnChecked="visible"; 
        
    else 
    
        blnChecked="hidden"; 
        
    document.getElementById(id).style.visibility=blnChecked; 
    
    //changeCheckAll(chkAllId,chkTagName,frm); 
}


function checkAll(chkAll,chkTagName,frm){ 
	var i; 
	var blnChecked=chkAll.checked; 
	
	for (i=0;i<frm.length-1;i++){ 
		e=frm.elements[i]; 
		if(e.type=='checkbox' && e.name.indexOf(chkTagName)!=-1){ 
			var checkBoxName=e.id; 
		
			e.checked=blnChecked; 
        }
		} 
	} 

function gettxtArray(txtSearch,frm){ 
	for (var i=0;i<frm.length;i++){ 
		e=frm.elements[i]; 
		if(e.type=='text' && e.id.indexOf(txtSearch)!=-1){ 
			var varArray=e.value.split(';'); 
			return varArray; 
		} 
	} 
}

function searchArrIndex(chkTagName,idArray){ 
    for(var i=0;i<idArray.length;i++){ 
        if (chkTagName.indexOf(idArray[i])!=-1) return i; 
    } 
} 

function hideObject(frm,chkTag,ctrlName,SignCount){ 
    var blnChecked; 
    if (chkTag.checked && SignCount>0) blnChecked="visible"; 
    else blnChecked="hidden"; 
    for(var i=0;i<frm.length;i++){ 
        e=frm.elements[i]; 
        if (e.type=='submit' && e.id.indexOf(ctrlName)!=-1){ 
            e.style.visibility=blnChecked; 
        } 
    } 
} 
function checkSelected(idName,alertMsg,confirmMsg,frm){ 
    var ctr=0; 
    for(var i=0;i<frm.length-1;i++){ 
        e=frm.elements[i]; 
        if(e.type=='checkbox' && e.name.indexOf(idName)!=-1){ 
            if(e.checked)ctr++; 
        } 
    } 
    if(ctr==0){
		alert(alertMsg);
		return false;
	} 
    else { 
        var reply=confirm('You have selected ' + ctr + '  item(s).\n' + confirmMsg); 
        if(reply==true)document.body.style.cursor='wait'; 
        return reply; 
    } 
} 

function checkAllSimpleAndTextBoxCSS(chkAll,chkTagName,txtName,txtCalendar,cssplain,frm){
	var blnChecked=chkAll.checked;
	for(var i=0;i<frm.length;i++){
		e=frm.elements[i];
		if(e.type=='checkbox' && e.name.indexOf(chkTagName)!=-1){
			e.checked=blnChecked;			
		}
		if(e.type=='text' && e.id.indexOf(txtName)!=-1){ 
			e.className=cssplain;
		}
		if(e.type=='text' && e.id.indexOf(txtCalendar)!=-1){ 
			e.className=cssplain;
		}
	}
}

function changeCheckAllAndTextBoxCSS(chkAllId,chkTagName,txtID,txtDateID,cssplain,frm){
	for(var i=0;i<frm.length;i++){
		e=frm.elements[i];
		if(e.type=='checkbox' && e.name.indexOf(chkTagName)!=-1){
			document.getElementById(txtID).className=cssplain;
			document.getElementById(txtDateID).className=cssplain;
			if (!e.checked){				
				document.getElementById(chkAllId).checked=false;
				return;
			}
		}
	}
	document.getElementById(chkAllId).checked=true;
}

function checkAllRowStatus(chkAll,chkTagName,ctrlNames,btnTargetID,frm){
	var blnChecked=chkAll.checked;
	document.getElementById(btnTargetID).disabled=!blnChecked;
	checkAllSimple(chkAll,chkTagName,frm);
	var arrCtrls=ctrlNames.split(';');
	for(var k=0;k<frm.length;k++){
		e=frm.elements[k];
		for(var i=0;i<arrCtrls.length;i++){
			if (e.type=='text' && e.name.indexOf(arrCtrls[i])!=-1){
				e.disabled=!blnChecked;
			}
		}
	}
}

function checkAllRowStatusImg(chkAll,chkTagName,ctrlNames,imgNames,btnTargetID,frm){
	disableImage(chkAll.checked,imgNames);
	checkAllRowStatus(chkAll,chkTagName,ctrlNames,btnTargetID,frm);
}

function checkItemRowStatus(chkTag,chkAllId,chkTagName,ctrlIDs,btnTargetID,frm){
	var blnChecked=chkTag.checked;
	changeCheckAllAndProceedButton(chkAllId,chkTagName,btnTargetID,frm);
	var arrCtrls=ctrlIDs.split(';');
	for(var i=0;i<arrCtrls.length;i++){
		document.getElementById(arrCtrls[i]).disabled=!blnChecked;		
	}
}

function changeCheckAllAndProceedButton(chkAllId,chkTagName,btnTargetID,frm){
	changeCheckAll(chkAllId,chkTagName,frm);
	enableTargetID(chkTagName,btnTargetID,frm);
}

function enableTargetID(chkTagName,btnTargetID,frm){
	for (var i=0;i<frm.length;i++){
			e=frm.elements[i];
			if(e.type=='checkbox' && e.name.indexOf(chkTagName)!=-1){
				if (e.checked){
					document.getElementById(btnTargetID).disabled=false;
					return;
				}
			}
		}
	document.getElementById(btnTargetID).disabled=true;
}

function enableTargetID_Disbursement(chkTagName,btnTargetID,frm){
	for (var i=0;i<frm.length;i++){
			e=frm.elements[i];
			if(e.type=='checkbox' && e.name.indexOf(chkTagName)!=-1){
				if (e.checked){
					document.getElementById(btnTargetID).disabled=false;
					return;
				}
			}
		}
	document.getElementById(btnTargetID).disabled=true;
}

function disableImage(blnChecked,imgNames){
	var arrImages=imgNames.split(';');
	for(var k=0;k<document.images.length;k++){
		e=document.images[k];
		for(var i=0;i<arrImages.length;i++){
			if (e.id.indexOf(arrImages[i])!=-1)e.disabled=!blnChecked;
		}
	}	
}

function checkAllSimpleAndProceedButton(chkAllId,chkTagName,btnTargetID,frm){
	checkAllSimple(chkAllId,chkTagName,frm);
	enableTargetID(chkTagName,btnTargetID,frm);
}

function resetGridRadioButton(rdoMe,rdoName,frm){
	for(var i=0;i<frm.length;i++){
		e=frm.elements[i];
		if(e.type=='radio' && e.name.indexOf(rdoName)!=-1 && e!=rdoMe){
			e.checked=false;
		}
	}
}

function checkDefaultPayee(rdoID){
	if (document.getElementById(rdoID).checked){
		alert("Default Payee cannot be Deleted.");
		return false
	}
	else return true
}

