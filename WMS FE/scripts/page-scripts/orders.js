async function getOrders() {
  return await httpReq(mainUrl + "GetOrders", {}, "GET");
}

let updateObj = {};

const httpURLObj = {
  delete: "Delete/Order/",
  get: "GetOrders/",
  update: "Update/Order/",
  post: "CreateNewOrder",
};

async function loadToTable(httpURLObj) {
  const headingArr = [
    "Date",
    "Scheduled date",
    "Execution date",
    "Order status",
    "Order type",
    "Customer name",
    "Created by user",
  ];
  createTheadTh(headingArr);

  // Tokie patys keys, kaip duomenų bazėje ir eiliškumas tas pats
  const objectKeysArr = [
    "date",
    "scheduledDate",
    "executionDate",
    "orderStatus",
    "orderType",
    "customerName",
    "createdByUser",
  ];

  const Orders = await getOrders();
  createTbody(Orders, objectKeysArr, httpURLObj, updateObj);
}

loadToTable(httpURLObj);

document.querySelector(".add-new-item").style.display = "block";

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

