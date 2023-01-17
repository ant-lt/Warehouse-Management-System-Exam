const httpURLObj = {
  delete: "Delete/Customer/",
  get: "GetCustomerBy/",
  update: "Update/Customer/",
  post: "CreateNewCustomer",
};

async function getCustomers() {
  return await httpReq(mainUrl + "GetCustomers", {}, "GET");
}

let updateObj = {};

async function loadToTable(httpURLObj) {
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

  // Tokie patys keys, kaip duomenų bazėje ir eiliškumas tas pats
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
  createTbody(customers, objectKeysArr, httpURLObj, updateObj);
}

loadToTable(httpURLObj);

// postas

document.querySelector(".add-new-item").addEventListener("click", (e) => {
  e.preventDefault();
  document.querySelector(".post-form").style.display = "block";
});

// prideda naujo iraso submita
document.querySelector(".submit-post")?.addEventListener("click", async (e) => {
  e.preventDefault();
  const postForm = document.querySelector(".post-form");
  let postFormData = new FormData(postForm);
  let postObj = {};

  postFormData.forEach((value, key) => {
    postObj[key] = value;
  });
  await httpReq(mainUrl + httpURLObj.post, postObj, "POST");
  document.querySelector(".post-form").style.display = "none";
  document.location.reload(true);
});
