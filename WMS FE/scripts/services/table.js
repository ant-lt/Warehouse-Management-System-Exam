/**
 *
 * 1 user pasirenka preke
 * 2 prekė prisideda prie orders
 * 3 useris nuėjas į krepšelį prideda visas prekes, gali ištrinti iš prekių sąrašo optional
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

// arr - duomenys iš db
// objectKeysArr - pasirinkti td, kurie bus atvaizduojami fronte
// updateObj - reikalingas norint padaryti update, pasiima iš formos duomenys paspaužiant update mygtuką
// bus padarytas update http requestas
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

    let productSelect = document.createElement("select");
    let quantityInput = document.createElement("input");

    let createOrderItemBtn = document.createElement("button");
    createOrderItemBtn.innerText = "Create order item";
    if (httpURLObj && httpURLObj.get) {

      if (window.location.pathname.includes("orders")) {
        quantityInput.setAttribute("input", "number");
        quantityInput.setAttribute("value", 1);
        const products = await httpReq(mainUrl + "GetProducts", {}, "GET");
        products.forEach(product => {
          let productOption = document.createElement("option");
          productOption.setAttribute("value", product.id)
          productOption.innerText = product.name

          productSelect.append(productOption)
        })

        trEl.append(productSelect, quantityInput, createOrderItemBtn)
      }

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

    // specifiniams puslapiams

    let submitBtn = document.createElement("button");
    
    if (window.location.pathname.includes("orders")) {
      submitBtn.innerText = "Submit order";
      submitBtn.setAttribute("class", "submit-order");
      trEl.append(submitBtn);
    }

    document.querySelector("tbody").append(trEl);

    // create order item

    createOrderItemBtn.addEventListener("click", async () => {
      const createOrderItemResponse = await httpReq(mainUrl + "CreateOrderItem", {
        orderId: obj.id,
        quantity: parseInt(quantityInput.value),
        productId: parseInt(productSelect.value)
      }, "POST");

      
      const registeredOrderResponse = await httpReq(mainUrl + "GetOrderBy/" + createOrderItemResponse.orderId, {
      }, "GET");


      let dateTd = document.createElement("td")
      dateTd.innerText = registeredOrderResponse.date

      let scheduleTd = document.createElement("td")
      scheduleTd.innerText = registeredOrderResponse.scheduledDate

      let executionDateTd = document.createElement("td")
      executionDateTd.innerText = registeredOrderResponse.executionDate

      let orderStatusTd = document.createElement("td")
      orderStatusTd.innerText = registeredOrderResponse.orderStatus

      let orderTypeTd = document.createElement("td")
      orderTypeTd.innerText = registeredOrderResponse.orderType

      let customerNameTd = document.createElement("td")
      customerNameTd.innerText = registeredOrderResponse.customerName

      
      let createdByUserTd = document.createElement("td")
      createdByUserTd.innerText = registeredOrderResponse.createdByUser

      let registeredTr = document.createElement("tr")
      registeredTr.append(dateTd, scheduleTd, orderStatusTd, orderTypeTd, customerNameTd, createdByUserTd)

      document.querySelector(".registered-orders tbody")
      .append(registeredTr)
      console.log("response: " + registeredOrderResponse)

      
    })

    // HTTP req events

    if (httpURLObj && httpURLObj.delete) {
      deleteBtn.addEventListener("click", async () => {
        await httpReq(mainUrl + httpURLObj.delete + obj.id, {}, "DELETE");
        document.location.reload(true);
      });
    }

    // Update
    if (httpURLObj && httpURLObj.update) {
      // Gaunami duomenys formai užpildyti

      if (window.location.pathname.includes("orders")) {
        updateBtn.addEventListener("click", async () => {
          const singleCustomer = await httpReq(mainUrl + "GetOrderBy/" + obj.id, {}, "GET");
          singleElByID = singleCustomer;

          let tmpId = "";
          document.querySelector(".update-form").innerHTML = "";
          Object.keys(singleElByID).forEach((key) => {

            if (key == "id") {
              tmpId = singleCustomer[key];
            } else if (key === "orderStatus") {

              let slecetStatus = document.createElement("select")
              slecetStatus.setAttribute("name", "orderStatusId")

              let optionStatusNew = document.createElement("option")
              optionStatusNew.setAttribute("value", 1)
              optionStatusNew.innerText = "New"

              let optionStatusCompleted = document.createElement("option")
              optionStatusCompleted.setAttribute("value", 2)
              optionStatusCompleted.innerText = "Completed"

              let optionStatusCanceled = document.createElement("option")
              optionStatusCanceled.setAttribute("value", 3)
              optionStatusCanceled.innerText = "Canceled"

              slecetStatus.append( optionStatusNew, optionStatusCompleted, optionStatusCanceled)

              document.querySelector(".update-form").append(slecetStatus)
            }
            else if (key === "orderType") {

              let slecetType = document.createElement("select")
              slecetType.setAttribute("name", "orderTypeId")

              let optionTypeInboud = document.createElement("option")
              optionTypeInboud.setAttribute("value", 1)
              optionTypeInboud.innerText = "Inbound"

              let optionTypeOutbound = document.createElement("option")
              optionTypeOutbound.setAttribute("value", 2)
              optionTypeOutbound.innerText = "Outbound"


              slecetType.append(optionTypeInboud, optionTypeOutbound)

              document.querySelector(".update-form").append(slecetType)
            }  else {
              let inputText = document.createElement("input");
              inputText.setAttribute("type", "text");
              inputText.setAttribute("name", key);
              inputText.setAttribute("value", singleElByID[key]);
              inputText.setAttribute("placeholder", key);
  
              document.querySelector(".update-form").append(inputText);

            }
          });

          let btn = document.createElement("button");
          btn.setAttribute("class", "submit-update");
          btn.append("submit");

          document.querySelector(".update-form").append(btn);

          btn.addEventListener("click", async (e) => {
            e.preventDefault();
            const updateForm = document.querySelector(".update-form");
            let updateFormData = new FormData(updateForm);
            let updateObj = {};

            updateFormData.forEach((value, key) => {
              updateObj[key] = value;
            });

            console.log("update url: ",mainUrl + "Update/Order/" + tmpId)
            console.log("update id: ", tmpId)
            console.log("update data: ", updateObj)

            const tmpTypeId = parseInt(updateObj.orderTypeId)
            const tmpStatusId = parseInt(updateObj.orderStatusId)

            updateObj.orderTypeId = tmpTypeId
            updateObj.orderStatusId = tmpStatusId

            const customerUpdateURL = mainUrl + "Update/Order/" + tmpId
            await httpReq(customerUpdateURL, updateObj, "PUT");
            // await httpReq(mainUrl + httpURLObj.delete + obj.id, {}, "DELETE");
          });
        });
      } else {
        updateBtn.addEventListener("click", async () => {
          const singleCustomer = await httpReq(mainUrl + httpURLObj.get + obj.id, {}, "GET");
          singleElByID = singleCustomer;

          document.querySelector(".update-form").innerHTML = "";

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
    }

    if (window.location.pathname.includes("orders") && document.querySelector(".submit-order")) {
      submitBtn.addEventListener("click", async () => {
        await httpReq(
          mainUrl + "SubmitOrder/" + obj.id,
          {
            orderId: obj.id,
          },
          "POST",
        );
        document.location.reload(true);
      });
    }
  });
}
