async function getProducts() {
  return await httpReq(mainUrl + "GetProducts", {}, "GET");
}

async function loadToTable() {
  const headingArr = ["Sku", "Name", "Description", "Volume", "Weigth"];
  createTheadTh(headingArr);

  // Tokie patys keys, kaip duomenų bazėje ir eiliškumas tas pats
  const objectKeysArr = ["sku", "name", "description", "volume", "weigth"];

  const products = await getProducts();
  createTbody(products, objectKeysArr);
}

loadToTable();
