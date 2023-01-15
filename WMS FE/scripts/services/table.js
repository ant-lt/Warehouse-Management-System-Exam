/**
 *
 * failu pakrovimo eiliškumas
 *
 * httpReq.js
 * table.js
 * puslapio-pavadinimas.js
 *
 */

function createTheadTh(arr) {
  let trEl = document.createElement("tr");

  arr.forEach((str) => {
    let th = document.createElement("th");
    th.append(str);
    trEl.append(th);
  });

  document.querySelector("thead").append(trEl);
}

function createTbody(arr, objectKeysArr, httpURLObj, singleElByID = {}) {
  document.querySelector("tbody").innerHTML = "";
  arr.forEach(async (obj) => {
    let trEl = document.createElement("tr");

    objectKeysArr.forEach((key) => {
      let td = document.createElement("td");

      td.append(obj[key]);
      trEl.append(td);
    });

    let updateBtn = document.createElement("button");
    if (httpURLObj && httpURLObj.get) {
      updateBtn.innerText = "Update";
      updateBtn.setAttribute("class", "update-btn");

      let updateTd = document.createElement("td");
      updateTd.append(updateBtn);
      trEl.append(updateTd);
    }

    let deleteBtn = document.createElement("button");
    if (httpURLObj && httpURLObj.delete) {
      deleteBtn.innerText = "Delete";
      deleteBtn.setAttribute("class", "delete-btn");

      let deleteTd = document.createElement("td");
      deleteTd.append(deleteBtn);
      trEl.append(deleteTd);
    }

    document.querySelector("tbody").append(trEl);

    // HTTP req events

    if (httpURLObj && httpURLObj.delete) {
      deleteBtn.addEventListener("click", async () => {
        await httpReq(mainUrl + httpURLObj.delete + obj.id, {}, "DELETE");
        document.location.reload(true);
      });
    }

    // update
    if (httpURLObj && httpURLObj.post) {
      const formInputs = await httpReq(mainUrl + httpURLObj.get + obj.id, {}, "GET");

      Object.keys(formInputs).forEach((key) => {
        let inputText = document.createElement("input");
        inputText.setAttribute("type", "text");
        inputText.setAttribute("name", key);
        inputText.setAttribute("value", singleElByID[key]);
        inputText.setAttribute("placeholder", key);

        document.querySelector(".post-form").append(inputText);
      });

      let btn = document.createElement("button");
      btn.setAttribute("class", "submit-post");
      btn.append("submit");

      document.querySelector(".post-form").append(btn);

      document.querySelector(".submit-post").addEventListener("click", async (e) => {
        const postForm = document.querySelector(".post-form");
        let postFormData = new FormData(postForm);
        let postObj = {};

        postFormData.forEach((value, key) => {
          postObj[key] = value;
        });
        await httpReq(mainUrl + httpURLObj.post + postObj.id, postObj, "POST");
      });
    }

    // update
    if (httpURLObj && httpURLObj.get) {
      // Gaunami duomenys formai užpildyti
      updateBtn.addEventListener("click", async () => {
        const singleCustomer = await httpReq(mainUrl + httpURLObj.get + obj.id, {}, "GET");
        singleElByID = singleCustomer;

        Object.keys(singleElByID).forEach((key) => {
          let inputText = document.createElement("input");
          inputText.setAttribute("type", "text");
          inputText.setAttribute("name", key);
          inputText.setAttribute("value", singleElByID[key]);
          inputText.setAttribute("placeholder", key);

          document.querySelector(".update-form").append(inputText);
        });

        let btn = document.createElement("button");
        btn.setAttribute("class", "submit-update");
        btn.append("submit");

        document.querySelector(".update-form").append(btn);

        document.querySelector(".submit-update").addEventListener("click", async (e) => {
          const updateForm = document.querySelector(".update-form");
          let updateFormData = new FormData(updateForm);
          let updateObj = {};

          updateFormData.forEach((value, key) => {
            updateObj[key] = value;
          });
          await httpReq(mainUrl + httpURLObj.update + updateObj.id, updateObj, "PUT");
        });
      });
    }
  });
}
