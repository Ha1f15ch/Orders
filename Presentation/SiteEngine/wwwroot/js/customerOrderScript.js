document.addEventListener('DOMContentLoaded', function () {
    const stars = document.querySelectorAll('.star');
    const ratingInput = document.getElementById('rating');

    stars.forEach((star) => {
        star.addEventListener('click', function () {
            const ratingValue = this.getAttribute('data-value');
            ratingInput.value = ratingValue;

            // Убрать выделение со всех звезд
            stars.forEach(star => star.classList.remove('selected'));

            // Выделить выбранные звезды
            for (let i = 0; i < ratingValue; i++) {
                stars[i].classList.add('selected');
            }
        });
    });
});
