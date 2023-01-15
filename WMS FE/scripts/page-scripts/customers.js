async function getCustomers() {
  return await httpReq(mainUrl + "GetCustomers", {}, "GET");
}

let updateObj = {};

async function loadToTable() {
  const headingArr = [
    "Name",
    "Legal Code",
    "Address",
    "City",
    "Post code",
    "Country",
    "Contact person",
    "Phone",
    "Email",
    "Status",
    "Created",
  ];
  createTheadTh(headingArr);

 
  const objectKeysArr = [
    "name",
    "legalCode",
    "address",
    "city",
    "postCode",
    "country",
    "contactPerson",
    "phoneNumber",
    "email",
    "status",
    "created",
  ];

  const customers = await getCustomers();
  const httpURLObj = {
    delete: "Delete/Customer/",
    get: "GetCustomerBy/",
    update: "Update/Customer/",
  };
  createTbody(customers, objectKeysArr, httpURLObj, updateObj);
}

loadToTable();

console.log("update", updateObj);
