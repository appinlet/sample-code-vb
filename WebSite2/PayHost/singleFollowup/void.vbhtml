﻿@*
    Void transaction example
*@

@Code
    'Key variables that are mandatory
    Dim pgid = PayhostSOAP.DEFAULT_PGID
    Dim reference = GlobalUtility.generateReference()
    Dim encryptionKey = PayhostSOAP.DEFAULT_ENCRYPTION_KEY
    Dim transId = ""
    Dim merchantOrderId = ""
    Dim voidResponse = ""
    Dim voidRequestText = ""

    'PayHost Web Service
    Dim payHOSTT As PayHost.PayHOST = New PayHost.PayHOSTClient("PayHOSTSoap11")
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

    'Make a SingleFollowUp call
    Dim voidRequest As PayHost.VoidRequestType = New PayHost.VoidRequestType()
    voidRequest.Account = New PayHost.PayGateAccountType()
    voidRequest.Account.PayGateId = pgid
    voidRequest.Account.Password = encryptionKey

    If (Not (Request("btnSubmit")) Is Nothing) Then
        Dim identifier = Request("identifier")
        Select Case (identifier)
            Case "merchantorderid"
                voidRequest.ItemElementName = New PayHost.ItemChoiceType2
                voidRequest.ItemElementName = payHOST.ItemChoiceType2.MerchantOrderId
                voidRequest.Item = Request("merchantOrderId")
            Case "transid"
                voidRequest.ItemElementName = New PayHost.ItemChoiceType2
                voidRequest.ItemElementName = payHOST.ItemChoiceType2.TransactionId
                voidRequest.Item = Request("transid")
        End Select

        voidRequest.TransactionType = New PayHost.TransactionType
        If ((Not (Request("transactionType")) Is Nothing) _
                    AndAlso (Request("transactionType") <> "")) Then
            voidRequest.TransactionType = CType([Enum].Parse(GetType(PayHost.TransactionType), Request("transactionType")), PayHost.TransactionType)
        End If

        voidRequestText = PayhostSOAP.getXMLText(voidRequest)

        Try
            Dim fupRequest = New PayHost.SingleFollowUpRequest With {
                .Item = voidRequest
            }
            Dim response = payHOSTT.SingleFollowUp(New PayHost.SingleFollowUpRequest1(fupRequest))
            Dim t = TryCast(response.SingleFollowUpResponse, PayHost.SingleFollowUpResponse)
            Dim status = TryCast(t.Items(0), PayHost.VoidResponseType)
            voidResponse = PayhostSOAP.getXMLText(response)
        Catch e As Exception
            Dim err = e.Message
        End Try
    End If
End Code

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <title>PayHost - Void</title>
    <link rel="stylesheet" href="../../lib/css/bootstrap.min.css">
    <link rel="stylesheet" href="../../lib/css/core.css">
</head>
<body>
    <div class="container-fluid" style="min-width: 320px;">
        <nav class="navbar navbar-inverse navbar-fixed-top">
            <div class="container-fluid">
                <!-- Brand and toggle get grouped for better mobile display -->
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="">
                        <img alt="PayGate" src="../../lib/images/paygate_logo_mini.png" />
                    </a>
                    <span style="color: #f4f4f4; font-size: 18px; line-height: 45px; margin-right: 10px;"><strong>PayHost Web Payment Void</strong></span>
                </div>
                <div class="collapse navbar-collapse" id="navbar-collapse">
                    <ul class="nav navbar-nav">
                        <li>
                            <a href="/../../PayHost/singlePayment/webPayment/index.vbhtml">Initiate</a>
                        </li>
                        <li class="active">
                            <a href="/../../PayHost/singleFollowUp/void.vbhtml">Void</a>
                        </li>
                        <li>
                            <a href="/../../PayHost/singleFollowUp/simple_void.vbhtml">Simple void</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <div style="background-color:#80b946; text-align: center; margin-top: 51px; margin-bottom: 15px; padding: 4px;"><strong>Void</strong></div>
        <div class="container">
            <form role="form" class="form-horizontal text-left" action="void.vbhtml" method="post">
                <div class="form-group">
                    <label for="payGateId" class="col-sm-3 control-label">PayGate ID</label>
                    <div class="col-sm-5">
                        <input class="form-control" type="text" name="payGateId" id="payGateId" value="@pgid" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="encryptionKey" class="col-sm-3 control-label">Encryption Key</label>
                    <div class="col-sm-5">
                        <input class="form-control" type="text" name="encryptionKey" id="encryptionKey" value="@encryptionKey" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="transactionType" class="col-sm-3 control-label">Transaction Type</label>
                    <div class="col-sm-5">
                        <div class="input-group">
                            <select name="transactionType" id="transactionType" class="form-control">
                                <option value="Authorisation">Authorisation</option>
                                <option value="Settlement">Settlement</option>
                                <option value="Refund">Refund</option>
                                <option value="Payout">Payout</option>
                                <option value="Purchase">Purchase</option>
                            </select>
                            @*<input type="text" name="transactionType" id="transactionType" class="form-control" aria-label="Transaction Type Input" value="@transactionType" />*@
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label for="merchantOrderId" class="col-sm-3 control-label">Merchant Order ID</label>
                    <div class="col-sm-5">
                        <div class="input-group">
                            <span class="input-group-addon">
                                <label for="merchantOrderIdChk" class="sr-only">Merchant Order ID Checkbox</label>
                                <input name="identifier" id="merchantOrderIdChk" value="merchantorderid" type="radio" aria-label="Merchant Order ID Checkbox">
                            </span>
                            <input type="text" name="merchantOrderId" id="merchantOrderId" class="form-control" aria-label="Merchant Order ID Input" value="@merchantOrderId" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label for="transid" class="col-sm-3 control-label">Transaction ID</label>
                    <div class="col-sm-5">
                        <div class="input-group">
                            <span class="input-group-addon">
                                <label for="transidChk" class="sr-only">Transaction ID Checkbox</label>
                                <input name="identifier" id="transidChk" value="transid" type="radio" aria-label="Transaction ID Checkbox">
                            </span>
                            <input type="text" name="transId" id="transId" class="form-control" aria-label="Transaction ID Input" value="@transId" />
                        </div>
                    </div>
                </div>
                <br>
                <div class="form-group">
                    <div class=" col-sm-offset-4 col-sm-4">
                        <img src="/../../lib/images/loader.gif" alt="Processing" class="initialHide" id="queryLoader">
                        <input class="btn btn-success btn-block" id="doVoidBtn" type="submit" name="btnSubmit" value="Do Void" />
                    </div>
                </div>
                <br>
            </form>
            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-offset-4 col-sm-4">
                    <button id="showRequestBtn" class="btn btn-primary btn-block" type="button" data-toggle="collapse" data-target="#requestDiv" aria-expanded="false" aria-controls="requestDiv">
                        Request
                    </button>
                </div>
            </div>
            <div id="requestDiv" class="row collapse well well-sm">
                <textarea rows="20" cols="100" id="request" class="form-control">@voidRequestText</textarea>
            </div>
            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-offset-4 col-sm-4">
                    <button id="showResponseBtn" class="btn btn-primary btn-block" type="button" data-toggle="collapse" data-target="#responseDiv" aria-expanded="false" aria-controls="responseDiv">
                        Response
                    </button>
                </div>
            </div>
            <div id="responseDiv" class="row collapse well well-sm">
                <textarea rows="20" cols="100" id="response" class="form-control">@voidResponse</textarea>

            </div>
        </div>
    </div>
    <script type="text/javascript" src="/../../lib/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="/../../lib/js/bootstrap.min.js"></script>
</body>
</html>
