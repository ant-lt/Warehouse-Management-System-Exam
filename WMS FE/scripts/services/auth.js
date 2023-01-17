if (localStorage.getItem("user-token") && localStorage.getItem("role")) {
  // jeigu prisijunges nauduotojas tada paslepia formą ir rodo, kad esate prisijunge
  if (window.location.pathname.includes("login") || window.location.pathname.includes("register")) {
    if (document.querySelector(".login-container")) {
      document.querySelector(".login-container").style.display = "none";
    }
    if (document.querySelector(".register-container")) {
      document.querySelector(".register-container").style.display = "none";
    }
    document.querySelector(".auth-message").style.display = "block";
  }

  // parodo logout linka
  if (document.querySelector(".logout")) {
    document.querySelector(".logout").style.display = "inline";

    // logout funkcija
    document.querySelector(".logout").addEventListener("click", () => {
      localStorage.removeItem("role");
      localStorage.removeItem("user-token");
      localStorage.removeItem("user-id");
      document.location.reload(true);
    });
  }

  // Jeigu prisijunges, nemato logino ir registracijos linkų
  if (document.querySelector(".login")) {
    document.querySelector(".login").style.display = "none";
  }

  if (document.querySelector(".register")) {
    document.querySelector(".register").style.display = "none";
  }

  // Rodyti puslapius pagal tam tikras role

  const role = localStorage.getItem("role");

  if (role === "Manager" || role === "Administrator") {
    document.querySelector(".customers").style.display = "inline";
  }

  if (role === "Manager" || role === "Administrator" || role === "Supervisor") {
    document.querySelector(".inventory").style.display = "inline";
    document.querySelector(".orders").style.display = "inline";
    document.querySelector(".products").style.display = "inline";
    document.querySelector(".reports").style.display = "inline";

    // parodo mygtuka prideti
    if (window.location.pathname.includes("customers")) {
      document.querySelector(".add-new-item").style.display = "block";
    }
  }
} else {
  // jeigu neprisijunges vartotojas nori pasiekti tam tikrus puslapius
  if (
    window.location.pathname.includes("customers") ||
    window.location.pathname.includes("inventory") ||
    window.location.pathname.includes("orders") ||
    window.location.pathname.includes("products") ||
    window.location.pathname.includes("cart") ||
    window.location.pathname.includes("reports")
  ) {
    document.querySelector("main").style.display = "none";
  }
}
