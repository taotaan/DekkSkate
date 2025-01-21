const imageContainer = document.querySelector('#imageContainer'); // เลือก container โดยใช้ id
let scrollAmount = 0;

function autoScroll() {
    scrollAmount += 0.5; // ปรับความเร็วของการเลื่อน
    imageContainer.scrollLeft = scrollAmount; // เลื่อน container

    // รีเซ็ตการเลื่อนเมื่อถึงจุดสิ้นสุด
    if (scrollAmount >= imageContainer.scrollWidth - imageContainer.clientWidth) {
        scrollAmount = 0;
    }

    requestAnimationFrame(autoScroll); // เรียกซ้ำเพื่อเลื่อนต่อเนื่อง
}

// เรียกใช้งานฟังก์ชัน
autoScroll();

const image = document.querySelector('#imageShow img');
const imageShow = document.querySelector('#imageShow');

// ฟังก์ชันจับ Scroll Event
window.addEventListener('scroll', () => {
    const scrollPosition = window.scrollY; // ดึงค่าการเลื่อนในแนวตั้ง
    const containerWidth = imageShow.offsetWidth; // ความกว้างของ container

    // คำนวณตำแหน่งภาพโดยแปลงการเลื่อนแนวตั้งเป็นการเลื่อนแนวนอน
    const newLeft = (scrollPosition % containerWidth) - containerWidth;
    image.style.left = `${newLeft}px`; // เลื่อนภาพตาม scroll
});
