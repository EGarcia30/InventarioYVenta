function openModal(id) {
    // Show the modal
    const modal = document.getElementById(`${id}`).classList.add("block");
}

function closeModal(id) {
    // Hide the modal
    const modal = document.getElementById(`${id}`).classList.remove("block");
}