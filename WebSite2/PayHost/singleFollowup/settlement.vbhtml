﻿@*
    Settlement transaction example
*@

@Code
    'Key variables that are mandatory
    Dim pgid = PayhostSOAP.DEFAULT_PGID
    Dim reference = GlobalUtility.generateReference()
    Dim encryptionKey = PayhostSOAP.DEFAULT_ENCRYPTION_KEY
    Dim transId = ""
    Dim merchantOrderId = ""
    Dim settleResponse = ""
    Dim settleRequestText = ""

    'PayHost Web Service
    Dim payHOSTT As PayHost.PayHOST = New PayHost.PayHOSTClient("PayHOSTSoap11")
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

    'Will make a SingleFollowUp call
    Dim settleRequest As PayHost.SettleRequestType = New PayHost.SettleRequestType()
    settleRequest.Account = New PayHost.PayGateAccountType()
    settleRequest.Account.PayGateId = pgid
    settleRequest.Account.Password = encryptionKey

    If Request("btnSubmit") IsNot Nothing Then
        Dim identifier = Request("identifier")

        Select Case identifier
            Case "merchantorderid"
                settleRequest.ItemElementName = New PayHost.ItemChoiceType()
                settleRequest.ItemElementName = payHOST.ItemChoiceType.MerchantOrderId
                settleRequest.Item = Request("merchantOrderId")
            Case "transid"
                settleRequest.ItemElementName = New PayHost.ItemChoiceType()
                settleRequest.ItemElementName = payHOST.ItemChoiceType.TransactionId
                settleRequest.Item = Request("transid")
        End Select

        settleRequestText = PayhostSOAP.getXMLText(settleRequest)

        Try
            Dim fupRequest = New PayHost.SingleFollowUpRequest With {
                .Item = settleRequest
            }
            Dim response = payHOSTT.SingleFollowUp(New PayHost.SingleFollowUpRequest1(fupRequest))
            Dim t = TryCast(response.SingleFollowUpResponse, PayHost.SingleFollowUpResponse)
            Dim status = TryCast(t.Items(0), PayHost.SettleResponseType)
            settleResponse = PayhostSOAP.getXMLText(response)
        Catch e As Exception
            Dim err = e.Message
        End Try
    End If
End Code

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <title>PayHost - Settlement</title>
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
                    <span style="color: #f4f4f4; font-size: 18px; line-height: 45px; margin-right: 10px;"><strong>PayHost Web Payment Settlement</strong></span>
                </div>
                <div class="collapse navbar-collapse" id="navbar-collapse">
                    <ul class="nav navbar-nav">
                        <li>
                            <a href="/../../PayHost/singlePayment/webPayment/index.vbhtml">Initiate</a>
                        </li>
                        <li class="active">
                            <a href="/../../PayHost/singleFollowUp/settlement.vbhtml">Settle</a>
                        </li>
                        <li>
                            <a href="/../../PayHost/singleFollowUp/simple_settlement.vbhtml">Simple settle</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <div style="background-color:#80b946; text-align: center; margin-top: 51px; margin-bottom: 15px; padding: 4px;"><strong>Settle</strong></div>
        <div class="container">
            <form role="form" class="form-horizontal text-left" action="settlement.vbhtml" method="post">
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
                        <input class="btn btn-success btn-block" id="doVoidBtn" type="submit" name="btnSubmit" value="Do Settle" />
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
                <textarea rows="20" cols="100" id="request" class="form-control">@settleRequestText</textarea>
            </div>
            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-offset-4 col-sm-4">
                    <button id="showResponseBtn" class="btn btn-primary btn-block" type="button" data-toggle="collapse" data-target="#responseDiv" aria-expanded="false" aria-controls="responseDiv">
                        Response
                    </button>
                </div>
            </div>
            <div id="responseDiv" class="row collapse well well-sm">
                <textarea rows="20" cols="100" id="response" class="form-control">@settleResponse</textarea>

            </div>
        </div>
    </div>
    <script type="text/javascript" src="/../../lib/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="/../../lib/js/bootstrap.min.js"></script>
</body>
</html>
