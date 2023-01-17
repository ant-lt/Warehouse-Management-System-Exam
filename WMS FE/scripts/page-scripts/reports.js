async function getReports() {
  return await httpReq(mainUrl + "GetWarehousesRatioOfOccupied", {}, "GET");
}

let updateObj = {};

async function loadToTable() {
  const headingArr = [
    "Id",
    "Warehouse name",
    "Warehouse description",
    "Warehouse location",
    "Warehouse total volume capacity",
    "Warehouse actual total occupied volume",
  ];
  createTheadTh(headingArr);

  // Tokie patys keys, kaip duomenų bazėje ir eiliškumas tas pats
  const objectKeysArr = [
    "id",
    "warehouseName",
    "warehouseDescription",
    "warehouseLocation",
    "warehouseTotalVolumeCapacity",
    "warehouseActualTotalOccupiedVolume",
  ];

  const reports = await getReports();

  createTbody(reports, objectKeysArr);
}

loadToTable();
