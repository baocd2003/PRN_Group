document.addEventListener("DOMContentLoaded", function () {
    const numOfLaborsInput = document.querySelector("#Project_NumOfLabors");
    const laborSalaryInput = document.querySelector("#Project_LaborSalaryPerMonth");
    const monthDurationInput = document.querySelector("#Project_MonthDuration");
    const totalMaterialPrice = document.querySelector("#Project_TotalMaterialPrice");
    const totalPriceInput = document.querySelector("#Project_TotalPrice");

    [numOfLaborsInput, laborSalaryInput, monthDurationInput, totalMaterialPrice].forEach(input => {
        input.addEventListener("input", calculateTotalPrice);
    });

    function calculateTotalPrice() {
        const numOfLabors = parseFloat(numOfLaborsInput.value);
        const laborSalary = parseFloat(laborSalaryInput.value);
        const monthDuration = parseFloat(monthDurationInput.value);
        const materialPriceValue = parseFloat(totalMaterialPrice.value);

        if (!isNaN(numOfLabors) && !isNaN(laborSalary) && !isNaN(monthDuration)) {
            const totalPrice = materialPriceValue + numOfLabors * laborSalary * monthDuration;
            totalPriceInput.value = totalPrice.toLocaleString();
        } else {
            totalPriceInput.value = 0;
        }
    }
});