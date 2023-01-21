async function getInventory() {
  return await httpReq(mainUrl + "GetInventories", {}, "GET");
}

let updateObj = {};

async function loadToTable() {
  const headingArr = [
    "Id",
    "Quantity",
    "Warehouse name",
    "Product name",
    "Product SKU",
    "Product description",
  ];
  createTheadTh(headingArr);

  // Tokie patys keys, kaip duomenų bazėje ir eiliškumas tas pats
  const objectKeysArr = [
    "id",
    "quantity",
    "warehouseName",
    "productName",
    "productSKU",
    "productDescription",
  ];

  const inventory = await getInventory();

  createTbody(inventory, objectKeysArr);
}

loadToTable();
