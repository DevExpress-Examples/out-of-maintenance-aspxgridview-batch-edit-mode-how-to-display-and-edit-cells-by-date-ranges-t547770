<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/BatchEditDateRanges/Default.aspx) (VB: [Default.aspx](./VB/BatchEditDateRanges/Default.aspx))
* [Default.aspx.cs](./CS/BatchEditDateRanges/Default.aspx.cs) (VB: [Default.aspx](./VB/BatchEditDateRanges/Default.aspx))
* [EditRange.aspx](./CS/BatchEditDateRanges/EditRange.aspx) (VB: [EditRange.aspx.vb](./VB/BatchEditDateRanges/EditRange.aspx.vb))
* **[EditRange.aspx.cs](./CS/BatchEditDateRanges/EditRange.aspx.cs) (VB: [EditRange.aspx.vb](./VB/BatchEditDateRanges/EditRange.aspx.vb))**
* [Model.cs](./CS/BatchEditDateRanges/Model.cs) (VB: [Model.vb](./VB/BatchEditDateRanges/Model.vb))
<!-- default file list end -->
# ASPxGridView - Batch Edit mode - How to display and edit cells by date ranges


This example illustrates how to implement a scenario where you need to edit numeric data related to some date within an undefined date range. I would like to comment on the three main parts.<br><br>1. Display date range data in the ASPxGridView control.<br><br>Date range data should be fetched from the data source first. You can do this in the <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.ASPxGridView.CustomUnboundColumnData.event">ASPxGridView.CustomUnboundColumnData</a> event by deserializing JSON data to the DateAmountMap type. This type encapsulates the functionality required to get and set a numeric value related to a particular date. See the Grid_CustomUnboundColumnData event handler in the <em>EditRange.aspx.cs</em> file and the DateAmountMap.GetDateAmount method in the <em>Model.cs</em> file.<br><br>2. Create date range-related Grid columns.<br><br>To separate different dates/months/years, use <a href="https://documentation.devexpress.com/AspNet/CustomDocument16143.aspx">band column</a>s. Such columns are created for the whole date range. Months are rendered by their names, not their numbers. Refer to the CreateDateColumns method in the <em>EditRange.aspx.cs</em> file.<br><br>3. Update date range data in the data source.<br><br>Data is parsed and serialized back to JSON in the <a href="https://documentation.devexpress.com/aspnet/DevExpressWebASPxGridBase_BatchUpdatetopic.aspx">ASPxGridView.BatchUpdate</a> event handler. Use the DateAmountMap.SetDateAmount method to set the required date value based on the DateFieldParts type's instance.<br><br>


<h3>Description</h3>

Date range data in this implementation is stored in a single database field as a JSON string. The structure of this string is as follows:<br>{<br>&nbsp; Years: {<br>&nbsp;&nbsp;&nbsp; 2016: {<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Months: {<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 10: {<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Days: {<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1: "0",<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 2: "1",<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ...<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; }<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; }<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1: { ... }<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; }<br>&nbsp;&nbsp;&nbsp; },<br>&nbsp;&nbsp;&nbsp; 2017: {<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ...<br>&nbsp;&nbsp;&nbsp; }<br>&nbsp; }<br>}

<br/>


