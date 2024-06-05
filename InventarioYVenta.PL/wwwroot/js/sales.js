const products = document.querySelectorAll("#Products");
let newNumber = 0;
let buyThings = [];

products.forEach((product) => {

    const numberInput = product.querySelector("#quantity-input");
    const amountBase = product.querySelector("#amountBase");
    const decrementBtn = product.querySelector("#decrement-button");
    const incrementBtn = product.querySelector("#increment-button");
    const saleBtn = product.querySelector(".sale");

    decrementBtn.addEventListener("click", () =>{
        if(parseInt(numberInput.value) == 0) return decrementBtn.classList.add("disabled");

        newNumber = parseInt(numberInput.value) - 1;
        numberInput.value = newNumber;
        return;
    });
    incrementBtn.addEventListener("click", () => {
        if(parseInt(numberInput.value) == parseInt(amountBase.textContent)) return incrementBtn.classList.add("disabled");

        newNumber = parseInt(numberInput.value) + 1;
        numberInput.value = newNumber;
        return;
    });

    saleBtn.addEventListener("click", (e) =>{
        e.preventDefault();

        if(e.target.classList.contains("sale") && parseInt(numberInput.value) != 0){
            readTheContent(product)
        }
    })
})

function readTheContent(product){
    const infoProduct = {
        InventoryId: product.querySelector(".inventoryId").value,
        Name: product.querySelector(".nameProduct").textContent,
        Amount: product.querySelector("#quantity-input").value,
        UnitPurchasePrice: product.querySelector(".unitPurchasePrice").value,
        UnitSalesPrice: product.querySelector(".unitSalesPrice").textContent,
        Status: product.querySelector(".status").value,
        CreatedAt: product.querySelector(".createdAt").value,
        UpdatedAt: product.querySelector(".updatedAt").value,
        DeletedAt: product.querySelector(".deletedAt").value
    }

    buyThings = [...buyThings,infoProduct];
    console.log(infoProduct);
}