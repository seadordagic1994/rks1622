function Validate(box) {
    if (box.value == "" ||
       (box.type == "password" && box.value.length < 8)) {
        box.style.borderColor = "red";
    }
    else {
        box.style.borderColor = "green";
    }
}