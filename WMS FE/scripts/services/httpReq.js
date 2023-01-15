// HTTP method implementation:
async function httpReq(url = "", data = {}, httpMethod) {
  // Default options are marked with *

  let response = "";

  if (httpMethod !== "GET") {
    response = await fetch(url, {
      method: httpMethod, // POST, PUT, DELETE, etc.
      headers: {
        Accept: "application/json, text/plain, */*",
        "Content-Type": "application/json",
        Authorization: "Bearer " + window.localStorage.getItem("user-token"),
      },

      body: JSON.stringify(data), // body data type must match "Content-Type" header
    });
  } else if (httpMethod === "DELETE") {
    await fetch(url, {
      method: httpMethod,
      headers: {
        Accept: "application/json, text/plain, */*",
        "Content-Type": "application/json",
        Authorization: "Bearer " + window.localStorage.getItem("user-token"),
      },
    });
    // GET
  } else {
    response = await fetch(url, {
      method: httpMethod, // *GET.
      headers: {
        Accept: "application/json, text/plain, */*",
        "Content-Type": "application/json",
        Authorization: "Bearer " + window.localStorage.getItem("user-token"),
      },
    });
  }

  if (httpMethod !== "DELETE") {
    return response.json();
  }
}
