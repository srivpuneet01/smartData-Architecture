/// <reference path="../Resources/dynamsoft.webtwain.intellisense.js" />
//--------------------------------------------------------------------------------------
//************************** Import Image*****************************
//--------------------------------------------------------------------------------------





/*-----------------select source---------------------*/
function source_onchange() {
    if (document.getElementById("divTwainType"))
        document.getElementById("divTwainType").style.display = "";
    if (document.getElementById("btnScan"))
        document.getElementById("btnScan").value = "Scan";

    if (document.getElementById("source"))
        DWObject.SelectSourceByIndex(document.getElementById("source").selectedIndex);
}


/*-----------------Acquire Image---------------------*/
function acquireImage() {
    DWObject.SelectSourceByIndex(document.getElementById("source").selectedIndex);
    DWObject.CloseSource();
    DWObject.OpenSource();
    DWObject.IfShowUI = document.getElementById("ShowUI").checked;

    var i;
    for (i = 0; i < 3; i++) {
        if (document.getElementsByName("PixelType").item(i).checked == true)
            DWObject.PixelType = i;
    }
    DWObject.Resolution = document.getElementById("Resolution").value;
    DWObject.IfFeederEnabled = document.getElementById("ADF").checked;
    DWObject.IfDuplexEnabled = document.getElementById("Duplex").checked;
    if (Dynamsoft.Lib.env.bWin || (!Dynamsoft.Lib.env.bWin && DWObject.ImageCaptureDriverType == 0))
        appendMessage("Pixel Type: " + DWObject.PixelType + "<br />Resolution: " + DWObject.Resolution + "<br />");

    DWObject.IfDisableSourceAfterAcquire = true;
    DWObject.AcquireImage();
}

/*-----------------Load Image---------------------*/
function btnLoad_onclick() {
    var OnSuccess = function() {
        appendMessage("Loaded an image successfully.<br/>");
        updatePageInfo();
    };

    var OnFailure = function(errorCode, errorString) {
        checkErrorStringWithErrorCode(errorCode, errorString);
    };
    
    DWObject.IfShowFileDialog = true;
    DWObject.LoadImageEx("", EnumDWT_ImageType.IT_ALL, OnSuccess, OnFailure);
}

function loadSampleImage(nIndex) {
    var ImgArr;

    switch (nIndex) {
        case 1:
            ImgArr = "/Images/twain_associate1.png";
            break;
        case 2:
            ImgArr = "/Images/twain_associate2.png";
            break;
        case 3:
            ImgArr = "/Images/twain_associate3.png";
            break;
    }

    if (location.hostname != '') {

        var OnSuccess = function() {
            appendMessage('Loaded a demo image successfully. (Http Download)<br/>');
            updatePageInfo();
        };

        var OnFailure = function(errorCode, errorString) {
            checkErrorStringWithErrorCode(errorCode, errorString);
        };
        
        DWObject.IfSSL = DynamLib.detect.ssl;
        var _strPort = location.port == "" ? 80 : location.port;
        if (DynamLib.detect.ssl == true)
            _strPort = location.port == "" ? 443 : location.port;
        DWObject.HTTPPort = _strPort;

        DWObject.HTTPDownload(location.hostname, DynamLib.getRealPath(ImgArr), OnSuccess, OnFailure);
    }
    else {
        var OnSuccess = function() {
            DWObject.IfShowFileDialog = true;

            appendMessage('Loaded a demo image successfully.');
            updatePageInfo();
        };

        var OnFailure = function(errorCode, errorString) {
            DWObject.IfShowFileDialog = true;
            checkErrorStringWithErrorCode(errorCode, errorString);
        };
        
        DWObject.IfShowFileDialog = false;
        DWObject.LoadImage(DynamLib.getRealPath(ImgArr), OnSuccess, OnFailure);
    }
}

//--------------------------------------------------------------------------------------
//************************** Edit Image ******************************

//--------------------------------------------------------------------------------------
function btnShowImageEditor_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.ShowImageEditor();
}

function btnRotateRight_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.RotateRight(DWObject.CurrentImageIndexInBuffer);
    appendMessage('<b>Rotate right: </b>');
    if (checkErrorString()) {
        return;
    }
}
function btnRotateLeft_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.RotateLeft(DWObject.CurrentImageIndexInBuffer);
    appendMessage('<b>Rotate left: </b>');
    if (checkErrorString()) {
        return;
    }
}

function btnRotate180_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.Rotate(DWObject.CurrentImageIndexInBuffer, 180, true);
    appendMessage('<b>Rotate 180: </b>');
    if (checkErrorString()) {
        return;
    }
}

function btnMirror_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.Mirror(DWObject.CurrentImageIndexInBuffer);
    appendMessage('<b>Mirror: </b>');
    if (checkErrorString()) {
        return;
    }
}
function btnFlip_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.Flip(DWObject.CurrentImageIndexInBuffer);
    appendMessage('<b>Flip: </b>');
    if (checkErrorString()) {
        return;
    }
}

/*----------------------Crop Method---------------------*/
function btnCrop_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    if (_iLeft != 0 || _iTop != 0 || _iRight != 0 || _iBottom != 0) {
        DWObject.Crop(
            DWObject.CurrentImageIndexInBuffer,
            _iLeft, _iTop, _iRight, _iBottom
        );
        _iLeft = 0;
        _iTop = 0;
        _iRight = 0;
        _iBottom = 0;
        appendMessage('<b>Crop: </b>');
        if (checkErrorString()) {
            return;
        }
        return;
    } else {
    appendMessage("<b>Crop: </b>failed. Please first select the area you'd like to crop.<br />");
    }
}
/*----------------Change Image Size--------------------*/
function btnChangeImageSize_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    switch (document.getElementById("ImgSizeEditor").style.visibility) {
        case "visible": document.getElementById("ImgSizeEditor").style.visibility = "hidden"; break;
        case "hidden": document.getElementById("ImgSizeEditor").style.visibility = "visible"; break;
        default: break;
    }
    document.getElementById("ImgSizeEditor").style.top = ds_gettop(document.getElementById("btnChangeImageSize")) + document.getElementById("btnChangeImageSize").offsetHeight + "px";
    document.getElementById("ImgSizeEditor").style.left = ds_getleft(document.getElementById("btnChangeImageSize")) - 30 + "px";

    var iWidth = DWObject.GetImageWidth(DWObject.CurrentImageIndexInBuffer);
    if (iWidth != -1)
        document.getElementById("img_width").value = iWidth;
    var iHeight = DWObject.GetImageHeight(DWObject.CurrentImageIndexInBuffer);
    if (iHeight != -1)
        document.getElementById("img_height").value = iHeight;
}
function btnCancelChange_onclick() {
    document.getElementById("ImgSizeEditor").style.visibility = "hidden";
}

function btnChangeImageSizeOK_onclick() {
    document.getElementById("img_height").className = "";
    document.getElementById("img_width").className = "";
    if (!re.test(document.getElementById("img_height").value)) {
        document.getElementById("img_height").className += " invalid";
        document.getElementById("img_height").focus();
        appendMessage("Please input a valid <b>height</b>.<br />");
        return;
    }
    if (!re.test(document.getElementById("img_width").value)) {
        document.getElementById("img_width").className += " invalid";
        document.getElementById("img_width").focus();
        appendMessage("Please input a valid <b>width</b>.<br />");
        return;
    }
    DWObject.ChangeImageSize(
        DWObject.CurrentImageIndexInBuffer,
        document.getElementById("img_width").value,
        document.getElementById("img_height").value,
        document.getElementById("InterpolationMethod").selectedIndex + 1
    );
    appendMessage('<b>Change Image Size: </b>');
    if (checkErrorString()) {
        document.getElementById("ImgSizeEditor").style.visibility = "hidden";
        return;
    }
}
//--------------------------------------------------------------------------------------
//************************** Save Image***********************************
//--------------------------------------------------------------------------------------
function btnSave_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    var i, strimgType_save;
    var NM_imgType_save = document.getElementsByName("imgType_save");
    for (i = 0; i < 5; i++) {
        if (NM_imgType_save.item(i).checked == true) {
            strimgType_save = NM_imgType_save.item(i).value;
            break;
        }
    }
    DWObject.IfShowFileDialog = true;
    var _txtFileNameforSave = document.getElementById("txt_fileNameforSave");
    _txtFileNameforSave.className = "";
    var bSave = false;

    var strFilePath = "C:\\" + _txtFileNameforSave.value + "." + strimgType_save;

    var OnSuccess = function() {
        appendMessage('<b>Save Image: </b>');
        checkErrorStringWithErrorCode(0, "Successful.");
    };

    var OnFailure = function(errorCode, errorString) {
        checkErrorStringWithErrorCode(errorCode, errorString);
    };

    var _chkMultiPageTIFF_save = document.getElementById("MultiPageTIFF_save");
    var vAsyn = false;
    if (strimgType_save == "tif" && _chkMultiPageTIFF_save.checked) {
        vAsyn = true;
        if ((DWObject.SelectedImagesCount == 1) || (DWObject.SelectedImagesCount == DWObject.HowManyImagesInBuffer)) {
            bSave = DWObject.SaveAllAsMultiPageTIFF(strFilePath, OnSuccess, OnFailure);
        }
        else {
            bSave = DWObject.SaveSelectedImagesAsMultiPageTIFF(strFilePath, OnSuccess, OnFailure);
        }
    }
    else if (strimgType_save == "pdf" && document.getElementById("MultiPagePDF_save").checked) {
        vAsyn = true;
        if ((DWObject.SelectedImagesCount == 1) || (DWObject.SelectedImagesCount == DWObject.HowManyImagesInBuffer)) {
            bSave = DWObject.SaveAllAsPDF(strFilePath, OnSuccess, OnFailure);
        }
        else {
            bSave = DWObject.SaveSelectedImagesAsMultiPagePDF(strFilePath, OnSuccess, OnFailure);
        }
    }
    else {
        switch (i) {
            case 0: bSave = DWObject.SaveAsBMP(strFilePath, DWObject.CurrentImageIndexInBuffer); break;
            case 1: bSave = DWObject.SaveAsJPEG(strFilePath, DWObject.CurrentImageIndexInBuffer); break;
            case 2: bSave = DWObject.SaveAsTIFF(strFilePath, DWObject.CurrentImageIndexInBuffer); break;
            case 3: bSave = DWObject.SaveAsPNG(strFilePath, DWObject.CurrentImageIndexInBuffer); break;
            case 4: bSave = DWObject.SaveAsPDF(strFilePath, DWObject.CurrentImageIndexInBuffer); break;
        }
    }

    if (vAsyn == false) {
        if (bSave)
            appendMessage('<b>Save Image: </b>');
        if (checkErrorString()) {
            return;
        }
    }
}
//--------------------------------------------------------------------------------------
//************************** Upload Image***********************************
//--------------------------------------------------------------------------------------
function btnUpload_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }

   
    for (var count = 0; count < DWObject.SelectedImagesCount; count++)
    {
    var i, strHTTPServer, strActionPage, strImageType;

    var _txtFileName = document.getElementById("txt_fileName");
    _txtFileName.className = "";
  
    //DWObject.MaxInternetTransferThreads = 5;
    strHTTPServer = location.hostname;
    DWObject.IfSSL = DynamLib.detect.ssl;
    var _strPort = location.port == "" ? 80 : location.port;
    if (DynamLib.detect.ssl == true)
        _strPort = location.port == "" ? 443 : location.port;
    DWObject.HTTPPort = _strPort;
    
    
    var CurrentPathName = unescape(location.pathname); // get current PathName in plain ASCII	
    var CurrentPath = CurrentPathName.substring(0, CurrentPathName.lastIndexOf("/") + 1);
    strActionPage = "/Scan/Home/SaveToFile"; //the ActionPage's file path , Online Demo:"SaveToDB.aspx" ;Sample: "SaveToFile.aspx";
    //  var strActionPage = CurrentPath + "Home/SaveToFile";
    var redirectURLifOK = CurrentPath + "online_demo_list.aspx";
    for (i = 0; i < 4; i++) {
        if (document.getElementsByName("ImageType").item(i).checked == true) {
            strImageType = i + 1;
            break;
        }
    }

    var uploadfilename = _txtFileName.value +"." + document.getElementsByName("ImageType").item(i).value ;
    uploadfilename = document.getElementById("HiddenTypeID").value + "_" + document.getElementById("HiddenType").value + "_" + i + "" + uploadfilename;
    var OnSuccess = function(httpResponse) {
        appendMessage('<b>Upload: </b>');
        checkErrorStringWithErrorCode(0, "Successful.");
        if (strActionPage.indexOf("SaveToFile") != -1) {
            alert("Successful")//if save to file.
        } else {
            window.location.href = redirectURLifOK;
        }
    };

    var OnFailure = function(errorCode, errorString, httpResponse) {
        checkErrorStringWithErrorCode(errorCode, errorString, httpResponse);
    };
    
    if (strImageType == 2 && document.getElementById("MultiPageTIFF").checked) {
        if ((DWObject.SelectedImagesCount == 1) || (DWObject.SelectedImagesCount == DWObject.HowManyImagesInBuffer)) {
            DWObject.HTTPUploadAllThroughPostAsMultiPageTIFF(
                strHTTPServer,
                strActionPage,
                uploadfilename,
                OnSuccess, OnFailure
            );
        }
        else {
            DWObject.HTTPUploadThroughPostAsMultiPageTIFF(
                strHTTPServer,
                strActionPage,
                uploadfilename,
                OnSuccess, OnFailure
            );
        }
    }
    else if (strImageType == 4 && document.getElementById("MultiPagePDF").checked) {
    if ((DWObject.SelectedImagesCount == 1) || (DWObject.SelectedImagesCount == DWObject.HowManyImagesInBuffer)) {
            DWObject.HTTPUploadAllThroughPostAsPDF(
                strHTTPServer,
                strActionPage,
                uploadfilename,
                OnSuccess, OnFailure
            );
        }
        else {
            DWObject.HTTPUploadThroughPostAsMultiPagePDF(
                strHTTPServer,
                strActionPage,
                uploadfilename,
                OnSuccess, OnFailure
            );
        }
    }
    else {
        DWObject.HTTPUploadThroughPostEx(
            strHTTPServer,
            DWObject.CurrentImageIndexInBuffer,
            strActionPage,
            uploadfilename,
            strImageType,
            OnSuccess, OnFailure
        );
    }
    //$.ajax({
    //    type: "POST",
    //    url: strActionPage,
    //    data: null,
    //    success: OnSuccess,
    //    dataType: "json"
        //});
    }
}

//--------------------------------------------------------------------------------------
//************************** Navigator functions***********************************
//--------------------------------------------------------------------------------------

function btnFirstImage_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.CurrentImageIndexInBuffer = 0;
    updatePageInfo();
}

function btnPreImage_wheel() {
    if (DWObject.HowManyImagesInBuffer != 0)
        btnPreImage_onclick()
}

function btnNextImage_wheel() {
    if (DWObject.HowManyImagesInBuffer != 0)
        btnNextImage_onclick()
}

function btnPreImage_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    else if (DWObject.CurrentImageIndexInBuffer == 0) {
        return;
    }
    DWObject.CurrentImageIndexInBuffer = DWObject.CurrentImageIndexInBuffer - 1;
    updatePageInfo();
}
function btnNextImage_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    else if (DWObject.CurrentImageIndexInBuffer == DWObject.HowManyImagesInBuffer - 1) {
        return;
    }
    DWObject.CurrentImageIndexInBuffer = DWObject.CurrentImageIndexInBuffer + 1;
    updatePageInfo();
}


function btnLastImage_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.CurrentImageIndexInBuffer = DWObject.HowManyImagesInBuffer - 1;
    updatePageInfo();
}

function btnRemoveCurrentImage_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.RemoveAllSelectedImages();
    if (DWObject.HowManyImagesInBuffer == 0) {
        document.getElementById("DW_TotalImage").value = DWObject.HowManyImagesInBuffer;
        document.getElementById("DW_CurrentImage").value = "";
        return;
    }
    else {
        updatePageInfo();
    }
}


function btnRemoveAllImages_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.RemoveAllImages();
    document.getElementById("DW_TotalImage").value = "0";
    document.getElementById("DW_CurrentImage").value = "";
}
function setlPreviewMode() {
    var varNum = parseInt(document.getElementById("DW_PreviewMode").selectedIndex + 1);
    var btnCrop = document.getElementById("btnCrop");
    if (btnCrop) {
        var tmpstr = btnCrop.src;
        if (varNum > 1) {
            tmpstr = tmpstr.replace('Crop.', 'Crop_gray.');
            btnCrop.src = tmpstr;
            btnCrop.onclick = function() { };
        }
        else {
            tmpstr = tmpstr.replace('Crop_gray.', 'Crop.');
            btnCrop.src = tmpstr;
            btnCrop.onclick = function() { btnCrop_onclick(); };
        }
    }

    DWObject.SetViewMode(varNum, varNum);
    if (Dynamsoft.Lib.env.bMac) {
        return;
    }
    else if (document.getElementById("DW_PreviewMode").selectedIndex != 0) {
        DWObject.MouseShape = true;
    }
    else {
        DWObject.MouseShape = false;
    }
}

//--------------------------------------------------------------------------------------
//*********************************radio response***************************************
//--------------------------------------------------------------------------------------
function rdTIFFsave_onclick() {
    var _chkMultiPageTIFF_save = document.getElementById("MultiPageTIFF_save");
    _chkMultiPageTIFF_save.disabled = false;
    _chkMultiPageTIFF_save.checked = false;

    var _chkMultiPagePDF_save = document.getElementById("MultiPagePDF_save");
    _chkMultiPagePDF_save.checked = false;
    _chkMultiPagePDF_save.disabled = true;
}
function rdPDFsave_onclick() {
    var _chkMultiPageTIFF_save = document.getElementById("MultiPageTIFF_save");
    _chkMultiPageTIFF_save.checked = false;
    _chkMultiPageTIFF_save.disabled = true;

    var _chkMultiPagePDF_save = document.getElementById("MultiPagePDF_save");
    _chkMultiPagePDF_save.disabled = false;
    _chkMultiPagePDF_save.checked = false;
}
function rdsave_onclick() {
    var _chkMultiPageTIFF_save = document.getElementById("MultiPageTIFF_save");
    _chkMultiPageTIFF_save.checked = false;
    _chkMultiPageTIFF_save.disabled = true;

    var _chkMultiPagePDF_save = document.getElementById("MultiPagePDF_save");
    _chkMultiPagePDF_save.checked = false;
    _chkMultiPagePDF_save.disabled = true;
}
function rdTIFF_onclick() {
    var _chkMultiPageTIFF = document.getElementById("MultiPageTIFF");
    _chkMultiPageTIFF.disabled = false;
    _chkMultiPageTIFF.checked = false;

    var _chkMultiPagePDF = document.getElementById("MultiPagePDF");
    _chkMultiPagePDF.checked = false;
    _chkMultiPagePDF.disabled = true;
}
function rdPDF_onclick() {
    var _chkMultiPageTIFF = document.getElementById("MultiPageTIFF");
    _chkMultiPageTIFF.checked = false;
    _chkMultiPageTIFF.disabled = true;
    
    var _chkMultiPagePDF = document.getElementById("MultiPagePDF");
    _chkMultiPagePDF.disabled = false;
    _chkMultiPagePDF.checked = false;

}
function rd_onclick() {
    var _chkMultiPageTIFF = document.getElementById("MultiPageTIFF");
    _chkMultiPageTIFF.checked = false;
    _chkMultiPageTIFF.disabled = true;
    
    var _chkMultiPagePDF = document.getElementById("MultiPagePDF");
    _chkMultiPagePDF.checked = false;
    _chkMultiPagePDF.disabled = true;
    
    var _imgTypejpeg2 = document.getElementById("imgTypejpeg2");
    _imgTypejpeg2.checked = true;
}



//--------------------------------------------------------------------------------------
//************************** Dynamic Web TWAIN Events***********************************
//--------------------------------------------------------------------------------------

function Dynamsoft_OnPostTransfer() {
    updatePageInfo();
}

function Dynamsoft_OnPostLoadfunction(path, name, type) {
    updatePageInfo();
}

function Dynamsoft_OnPostAllTransfers() {
    updatePageInfo();
    checkErrorString();
}

function Dynamsoft_OnMouseClick(index) {
    updatePageInfo();
}

function Dynamsoft_OnMouseRightClick(index) {
    // To add
}


function Dynamsoft_OnImageAreaSelected(index, left, top, right, bottom) {
    _iLeft = left;
    _iTop = top;
    _iRight = right;
    _iBottom = bottom;
}

function Dynamsoft_OnImageAreaDeselected(index) {
    _iLeft = 0;
    _iTop = 0;
    _iRight = 0;
    _iBottom = 0;
}

function Dynamsoft_OnMouseDoubleClick() {
    return;
}


function Dynamsoft_OnTopImageInTheViewChanged(index) {
    _iLeft = 0;
    _iTop = 0;
    _iRight = 0;
    _iBottom = 0;
    DWObject.CurrentImageIndexInBuffer = index;
    updatePageInfo();
}
var Dst = 1;
function Dynamsoft_OnGetFilePath(bSave, count, index, path, name) {
}
//Upload File in MVC
//Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', Dynamsoft_OnReady);
//dynamsoft.WebTwainEnv.RegisterEvent()
//function Dynamsoft_OnReady() {
//   // DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');


   
//}

//new methods
function OnSuccess() {
  //  $('#loadingmessage').hide();
    console.log('successful');
}
function OnFailure(errorCode, errorString) {
  //  $('#loadingmessage').hide();
    alert(errorString);
}

function OnHttpUploadSuccess() {

    if (Dst <= 0) {
        SessionDestroy();
    }
    else {
        Dst = Dst - 1;
    }
   // $('#loadingmessage').hide();
  
    console.log('successful');
}

function OnHttpUploadFailure(errorCode, errorString, sHttpResponse) {
   // $('#loadingmessage').hide();
    alert(errorString + sHttpResponse);
}


function Dynamsoft_OnPostAllTransfers() {
    // DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');
    if (document.getElementById("HiddenTypeID").value == "" || document.getElementById("HiddenTypeID").value == "Select a User" || document.getElementById("HiddenTypeID").value == "Select a Patient") {
        alert("Select Associate Type")
        return false;

    }
    if (DWObject) {
     //   if (DWObject.HowManyImagesInBuffer > 0) {
        //   for (var count = 0; count < DWObject.SelectedImagesCount; count++) {
        var count = DWObject.SelectedImagesCount;
           // $('#loadingmessage').show();
                var strHTTPServer = location.hostname;
                DWObject.HTTPPort = location.port == "" ? 80 : location.port;
                var CurrentPathName = unescape(location.pathname); // get current PathName in plain ASCII
                var CurrentPath = CurrentPathName.substring(0, CurrentPathName.lastIndexOf("/") + 1);
                var strActionPage = "/Scan/Home/SaveToFile";//CurrentPath + "SaveToFile";
                var _txtFileName = document.getElementById("txt_fileName");
                //  var uploadfilename = _txtFileName.value + "." + document.getElementsByName("ImageType").item(0).value;
                var uploadfilename = _txtFileName.value + "." +"PDF";
                var uploadfilename = count + uploadfilename;

             //   DWObject.HTTPUploadAllThroughPostAsPDF(strHTTPServer, strActionPage, document.getElementById("HiddenTypeID").value + "_" + 'S' + document.getElementById("HiddenType").value + "_" + uploadfilename, OnHttpUploadSuccess, OnHttpUploadFailure);
                if (DWObject.ErrorCode == 0) {
                    // alert("File Uploaded!");
                   
                    
                }
              
            }
       // }
   // }
}
function DWT_AcquireImage() {
    DWObject.SelectSource();
    DWObject.OpenSource();
    DWObject.IfShowUI = false;
    DWObject.IfFeederEnabled = true;
    DWObject.IfAutoFeed = true;
    DWObject.XferCount = -1;
    DWObject.AcquireImage(); //using ADF  for scanning
}
function DynamicWebTwain_OnPostTransfer() { //fires after each scan
    var strFileName;
    var Digital = new Date();
    var Month = Digital.getMonth() + 1;
    var Day = Digital.getDate();
    var Hour = Digital.getHours();
    var Minute = Digital.getMinutes();
    var Second = Digital.getSeconds();
    var CurrentTime = Month + "_" + Day + "_" + Hour + "_" + Minute + "_" + Second;
    strFileName = "D:/temp/" + CurrentTime + ".pdf";
    DWObject.SaveAsPDF(strFileName, DWObject.CurrentImageIndexInBuffer); //save each scanned image as a different PDF file 
    if (DWObject.ErrorCode != 0) {
        alert(DWObject.ErrorString);
    }
}
//InsertBasic Record
function btnUpload11_onclick() {


    if (document.getElementById("HiddenTypeID").value == "" || document.getElementById("HiddenTypeID").value == "Select a User" || document.getElementById("HiddenTypeID").value == "Select a Patient") {
        alert("Select Associate Type")
        return false;

    }
    //document.getElementById("HiddenTypeID").value + "_" + document.getElementById("HiddenType").value
    var i = 0;
    var _strServerName = location.hostname; //Demo: "www.dynamsoft.com";
    var _strPort = location.port == "" ? 80 : location.port; //Demo: 80;
    var _currentPathName = unescape(location.pathname); // get current PathName in plain ASCII     
    var _currentPath = _currentPathName.substring(0, _currentPathName.lastIndexOf("/") + 1);
    //  var _strActionPage = _currentPath + "/Scan/Home/SaveToFile";
    var dataToSend = "01";
    //  var url = '/controllerName/actionName/' + dataToSend;
    var _strActionPage = "/Scan/Home/SaveToFile";
    DWObject.HTTPPort = _strPort;
    var strHostIP = location.hostname
    var _txtFileName = document.getElementById("txt_fileName");

    DWObject.HTTPUploadThroughPost(strHostIP, i, _strActionPage,"abc.png");
    //var AssociatedWith = document.getElementById("HiddenTypeID").value;
    //var Type = document.getElementById("HiddenType").value;
     
    //    $('#loadingmessage').show();
    //    $.ajax({
    //        type: "POST",
    //        url: "/Scan/Home/Index/SaveToFile",
    //        data: null,
    //       // data: '{AssociatedWith: "' + AssociatedWith + '",Type: ' + Type + '}',//AddScannedRecord
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (data) {
    //           // $('#loadingmessage').hide();
               
    //            alert(data);
               
    //        },
    //        failure: function (response) {
    //            alert(response.d);
    //        }
    //    });

    
}
//
function btnUpload1_onclick() {
    //debugger;
    //alert("Files");
    $("#loadingmessage").show();
    var i;
    if (document.getElementById("HiddenTypeID").value == "" || document.getElementById("HiddenTypeID").value == "Select a User" || document.getElementById("HiddenTypeID").value == "Select a Patient")
    {
        alert("Select Associate Type");
        return false;

    }
    var _txtFileName = document.getElementById("txt_fileName").value;
    if (_txtFileName == "")
    {
        alert("Enter File Name");
        return false;
    }

    //if (document.getElementsByName("ImageType").value == undefined || document.getElementsByName("ImageType").value == "")
    //{
    //    alert("Select Image Type");
    //    return false;
    //}

    var _strServerName = location.hostname; //Demo: "www.dynamsoft.com";
    var _strPort = location.port == "" ? 80 : location.port; //Demo: 80;
    var _currentPathName = unescape(location.pathname); // get current PathName in plain ASCII     
    var _currentPath = _currentPathName.substring(0, _currentPathName.lastIndexOf("/") + 1);
    //  var _strActionPage = _currentPath + "/Scan/Home/SaveToFile";
    var dataToSend = "01";
  //  var url = '/controllerName/actionName/' + dataToSend;
    var _strActionPage = "/Scan/Home/SaveToFile";
    DWObject.HTTPPort = _strPort;
    var strHostIP = location.hostname
    var _txtFileName = document.getElementById("txt_fileName");
    _txtFileName.className = "";
  
     for (i = 0; i < 4; i++) {
        if (document.getElementsByName("ImageType").item(i).checked == true) {
            strImageType = i + 1;
            break;
        }
    }

     var uploadfilename = _txtFileName.value + "." + document.getElementsByName("ImageType").item(i).value;
   
     for (i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
        // $("#loadingmessage").show();
        DWObject.HTTPUploadThroughPost(strHostIP, i, _strActionPage, document.getElementById("HiddenTypeID").value + "_" + document.getElementById("HiddenType").value + "_" + i + "" + uploadfilename);
       
    }
    if (i == DWObject.HowManyImagesInBuffer) {
        if (DWObject.HowManyImagesInBuffer > 0) { 
            $("#loadingmessage").hide();
         //   var _strActionPage1 = "/Scan/Home/DestroySession";
         //   DWObject.HTTPUploadThroughPost(strHostIP, i, _strActionPage1, "abc.png");
            alert("File Uploaded!");
            SessionDestroy();
            if (Dst <= 0)
            {

             //   SessionDestroy();
            }
            else
                {
                Dst = Dst - 1;
            }
            //location.reload();
        }
       // $("#loadingmessage").hide();
      //  $("#AddModal").hide();
    }
}

//Ajax Call

function SessionDestroy() {
    var _strServerName = location.hostname; //Demo: "www.dynamsoft.com";
    var _strPort = location.port == "" ? 80 : location.port; //Demo: 80;
    var _currentPathName = unescape(location.pathname);
    var _currentPath = _currentPathName.substring(0, _currentPathName.lastIndexOf("/") + 1);
    var _strActionPage = "/Scan/Home/DestroySession";
    var _uploadHeaders = [];
    var _uploadHeader = {
        key: "Content-Type",
        value: "application/x-www-form-urlencoded"
    }
    _uploadHeaders.push(_uploadHeader);
    DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
    //for (var i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
    for (var i = 0; i < 1; i++) {
        DWObject.SetSelectedImageIndex(i, i);
    }
    var size = DWObject.GetSelectedImagesSize(EnumDWT_ImageType.IT_PDF);
    var imagedata = DWObject.SaveSelectedImagesToBase64Binary();
    var uploadData = encodeURIComponent("ImageName") + "=" + encodeURIComponent("testAJAXUpload.pdf")
        + "&" + encodeURIComponent("RemoteFile") + "=" + encodeURIComponent(imagedata);
    var ajax_request = createXMLHttpRequest();
    requestData(ajax_request, _strActionPage, uploadData, null, "POST", _uploadHeaders);
    Dst = Dst + 1;
}


function CreateSession() {
    //debugger;
    var _strServerName = location.hostname; //Demo: "www.dynamsoft.com";
    var _strPort = location.port == "" ? 80 : location.port; //Demo: 80;
    var _currentPathName = unescape(location.pathname);
    var _currentPath = _currentPathName.substring(0, _currentPathName.lastIndexOf("/") + 1);
    var _strActionPage = "/Scan/Home/AddSession";
    var _uploadHeaders = [];
    var _uploadHeader = {
        key: "Content-Type",
        value: "application/x-www-form-urlencoded"
    }
    _uploadHeaders.push(_uploadHeader);
    DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
    //for (var i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
    for (var i = 0; i < 1; i++) {
        DWObject.SetSelectedImageIndex(i, i);
    }
    //for (i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
    //    DWObject.HTTPUploadThroughPost(strHostIP, i, _strActionPage, document.getElementById("HiddenTypeID").value + "_" + document.getElementById("HiddenType").value + "_" + i + "" + uploadfilename);

    //}


   

    for (i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
        var size = DWObject.GetSelectedImagesSize(EnumDWT_ImageType.IT_PDF);
        var TypeId = document.getElementById("HiddenTypeID").value;
        var Type = document.getElementById("HiddenType").value;
        var imagedata = DWObject.SaveSelectedImagesToBase64Binary();
        var _txtFileName = document.getElementById("txt_fileName");
        var uploadfilename = _txtFileName.value + "." + document.getElementsByName("ImageType").item(i).value;
        var uploadData = encodeURIComponent("ImageName") + "=" + encodeURIComponent(uploadfilename)
            + "&" + encodeURIComponent("RemoteFile") + "=" + encodeURIComponent(imagedata) + "&" + encodeURIComponent("TypeId") + "=" + encodeURIComponent(TypeId) + "&" + encodeURIComponent("Type") + "=" + encodeURIComponent(Type);
        var ajax_request = createXMLHttpRequest();
        requestData(ajax_request, _strActionPage, uploadData, null, "POST", _uploadHeaders);
    }
    if (i == DWObject.HowManyImagesInBuffer) {
        if (DWObject.HowManyImagesInBuffer > 0) {

           // var _strActionPage1 = "/Scan/Home/DestroySession";
          //  DWObject.HTTPUploadThroughPost(strHostIP, i, _strActionPage1, "abc.png");
            alert("File Uploaded!");
            SessionDestroy();
            //if (Dst <= 0) {
              
            //}
            //else {
            //    Dst = Dst - 1;
            //}
            //location.reload();
        }
       // $('#loadingmessage').hide();
    }
   // Dst = Dst + 1;
}


function createXMLHttpRequest() {
    var request = false;
    /* Does this browser support the XMLHttpRequest object? */
    if (window.XMLHttpRequest) {
        if (typeof XMLHttpRequest != 'undefined')
            /* Try to create a new XMLHttpRequest object */
            try {
                request = new XMLHttpRequest();
            } catch (e) {
                request = false;
            }
        /* Does this browser support ActiveX objects? */
    } else if (window.ActiveXObject) {
        /* Try to create a new ActiveX XMLHTTP object */
        try {
            request = new ActiveXObject('Msxml2.XMLHTTP');
        } catch (e) {
            try {
                request = new ActiveXObject('Microsoft.XMLHTTP');
            } catch (e) {
                request = false;
            }
        }
    }
    return request;
}
function requestData(p_request, p_URL, p_data, p_func, p_method, p_headers) {
    /* Does the XMLHttpRequest object exist? */
    if (p_request) {
        /*Is the posting method 'GET'? */
        if (p_method == 'GET') {
            p_request.open('GET', p_URL + '?' + p_data, true);
        }
        else {
            p_request.open('POST', p_URL, true);
        }
        p_request.onreadystatechange = p_func;
        /* Is the posting method 'GET'? */
        if (p_method == 'GET') {
            p_request.send(null);
        }
        else {
            if (typeof p_headers != "undefined") {
                for (var i = 0; i < p_headers.length; i++) {
                    var o = p_headers[i];
                    p_request.setRequestHeader(o.key, o.value);
                }
            }
            p_request.send(p_data);
        }
    }
}
//Show Images In EditMode
function LoadImagesInEditMode(data) {

    for (i = 0; i < data.length; i++) {
       DWObject.HTTPDownload("http://localhost:55038/", "/Areas/Scan/UploadedImages/" + data[i].ImageName);
    }
  //  $("#AddModal").modal('show');
   // $("#loadingmessage").hide();
}
function btnHTTPDownload() {
    DWObject.HTTPDownload("http://localhost:55038/", "/Areas/Scan/UploadedImages/26_U_0WebTWAINImage.jpg");
}
//End
