const imageContainer = document.querySelector('.image-container');
let scrollAmount = 0;

function autoScroll() {
    scrollAmount += 0.5; // ความเร็วในการเลื่อน (ปรับได้)
    imageContainer.scrollLeft = scrollAmount;

    if (scrollAmount >= imageContainer.scrollWidth - imageContainer.clientWidth) {
        scrollAmount = 0;
    }

    requestAnimationFrame(autoScroll);
}

autoScroll();
