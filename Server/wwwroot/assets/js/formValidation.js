export const registerAccount = () => {
  const emailInput = document.querySelector("#email");
  const emailLabel = document.querySelector("#emailFeedback");
  const nameInput = document.querySelector("#name");
  const nameLabel = document.querySelector("#nameFeedback");
  const passwordInput = document.querySelector("#password");
  const passwordLabel = document.querySelector("#passwordFeedback");
  const repeatPasswordInput = document.querySelector("#repeatPassword");
  const repeatPasswordLabel = document.querySelector("#repeatPasswordFeedback");

  const successMessage = document.querySelector("#successMessage");
  const errorMessage = document.querySelector("#errorMessage");
  const form = document.querySelector("#registerForm");

  emailInput.addEventListener("input", () => {
    const validatedEmail = validateEmail(emailInput.value.trim());

    if (!validatedEmail.isValid) {
      emailLabel.innerHTML = validatedEmail.message;
    } else {
      emailLabel.innerHTML = "";
    }
  });

  nameInput.addEventListener("input", () => {
    const validatedName = validateName(nameInput.value.trim());

    if (!validatedName.isValid) {
      nameLabel.innerHTML = validatedName.message;
    } else {
      nameLabel.innerHTML = "";
    }
  });

  passwordInput.addEventListener("input", () => {
    const validatedPassword = validatePassword(passwordInput.value.trim());

    if (!validatedPassword.isValid) {
      passwordLabel.innerHTML = validatedPassword.message;
    } else {
      passwordLabel.innerHTML = "";
    }
  });

  repeatPasswordInput.addEventListener("input", () => {
    const validatedRepeatPassword = validateRepeatPassword(
      passwordInput.value.trim(),
      repeatPasswordInput.value.trim()
    );

    if (!validatedRepeatPassword.isValid) {
      repeatPasswordLabel.innerHTML = validatedRepeatPassword.message;
    } else {
      repeatPasswordLabel.innerHTML = "";
    }
  });

  form.addEventListener("submit", (e) => {
    e.preventDefault();

    // One last validation before post, return user object if successful.
    const user = validateForm(
      emailInput.value,
      emailLabel,
      nameInput.value,
      nameLabel,
      passwordInput.value,
      passwordLabel,
      repeatPasswordInput.value,
      repeatPasswordLabel
    );

    if (user === undefined) {
      return;
    }

    // if user is not undefined, proceed with api call to backend.
    fetch("https://localhost:7264/api/users/register", {
      method: "POST",
      body: JSON.stringify(user),
      headers: {
        "Content-type": "application/json; charset=UTF-8",
      },
    }).then(async (response) => {
      let data;
      const contentType = response.headers.get("content-type");

      // check format of the response to parse it appropriately
      if (contentType?.includes("application/json")) {
        data = await response.json();
      } else {
        data = await response.text();
      }
      if (response.ok) {
        successMessage.innerHTML = data;
        errorMessage.innerHTML = "";
        emailInput.value = "";
        nameInput.value = "";
        passwordInput.value = "";
        repeatPasswordInput.value = "";
      } else {
        if (Array.isArray(data)) {
          const messages = data.map((err) => err.errorMessage).join("<br>");
          errorMessage.innerHTML = messages;
        } else if (typeof data === "string") {
          errorMessage.innerHTML = data;
        } else {
          errorMessage.innerHTML = "Ett okänt fel inträffade.";
          successMessage.innerHTML = "";
        }
      }
    });
  });
};

const validateForm = (
  email,
  emailLabel,
  name,
  nameLabel,
  password,
  passwordLabel,
  repeatPassword,
  repeatPasswordLabel
) => {
  const validatedEmail = validateEmail(email.trim());
  const validatedName = validateName(name.trim());
  const validatedPassword = validatePassword(password.trim());
  const validatedRepeatPassword = validateRepeatPassword(
    password.trim(),
    repeatPassword.trim()
  );

  if (!validatedEmail.isValid) {
    emailLabel.innerHTML = validatedEmail.message;
    return;
  }
  if (!validatedName.isValid) {
    nameLabel.innerHTML = validatedName.message;
    return;
  }
  if (!validatedPassword.isValid) {
    passwordLabel.innerHTML = validatedPassword.message;
    return;
  }
  if (!validatedRepeatPassword.isValid) {
    repeatPasswordLabel.innerHTML = validatedRepeatPassword.message;
    return;
  }
  return { Email: email, Username: name, Password: password };
};

const validateEmail = (email) => {
  const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  if (email.length === 0) {
    return { isValid: false, message: "En epost krävs" };
  } else if (!emailRegex.exec(email)) {
    return { isValid: false, message: "Ange en giltig epost" };
  }

  return { isValid: true, message: "" };
};

const validateName = (name) => {
  const nameRegex = /^[a-zA-Z]\w{1,30}$/;

  if (name.length === 0) {
    return { isValid: false, message: "Användarnamn får inte vara tomt" };
  } else if (!nameRegex.exec(name)) {
    return {
      isValid: false,
      message:
        "Användarnamn får endast innehålla bokstäver, siffror och understreck",
    };
  }
  return { isValid: true, message: "" };
};

const validatePassword = (password) => {
  const passwordRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$/;

  if (email.length < 8 || !passwordRegex.exec(password)) {
    return {
      isValid: false,
      message:
        "Lösenordet måste innehålla minst 8 tecken, en liten bokstav, en stor bokstav, ett specialtecken samt en siffra.",
    };
  }
  return { isValid: true, message: "" };
};

const validateRepeatPassword = (password, repeatPassword) => {
  if (password !== repeatPassword) {
    return { isValid: false, message: "Lösenorden måste matcha" };
  }
  return { isValid: true, message: "" };
};
