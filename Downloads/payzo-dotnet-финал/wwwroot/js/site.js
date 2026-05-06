// PAYZO – site.js (shared frontend utilities)

function openModal(id) {
  const m = document.getElementById(id);
  if (m) m.classList.add('open');
}
function closeModal(id) {
  const m = document.getElementById(id);
  if (m) m.classList.remove('open');
}

// Close on backdrop click or X button
document.addEventListener('click', e => {
  if (e.target.classList.contains('modal-backdrop')) e.target.classList.remove('open');
  if (e.target.classList.contains('modal-close'))    e.target.closest('.modal-backdrop')?.classList.remove('open');
});

// Auto-dismiss alerts after 4s
document.querySelectorAll('.alert.show').forEach(el => {
  setTimeout(() => el.style.opacity = '0', 3500);
  setTimeout(() => el.remove(), 4000);
});

// Progress bar color helper
function progressColor(pct) {
  if (pct >= 100) return '#dc2626';
  if (pct >= 75)  return '#f59e0b';
  return '#16a34a';
}

// Emoji picker
function pickEmoji(hiddenId, pickerId, btn, emoji) {
  // Set hidden input value
  document.getElementById(hiddenId).value = emoji;
  // Remove selected from all buttons in this picker
  document.getElementById(pickerId).querySelectorAll('.emoji-btn')
    .forEach(b => b.classList.remove('selected'));
  // Mark this one selected
  btn.classList.add('selected');
}

// When edit modal opens, highlight the current emoji
function syncEmojiPicker(pickerId, currentEmoji) {
  const picker = document.getElementById(pickerId);
  if (!picker) return;
  picker.querySelectorAll('.emoji-btn').forEach(b => {
    b.classList.toggle('selected', b.textContent.trim() === currentEmoji);
  });
  // Scroll selected into view
  const sel = picker.querySelector('.emoji-btn.selected');
  if (sel) sel.scrollIntoView({ block: 'nearest' });
}
