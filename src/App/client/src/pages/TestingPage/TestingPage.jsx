import React, { useEffect } from "react";
import KUTE from "kute.js";
import MEDitor from "@uiw/react-md-editor";
import { FaRegStickyNote } from "react-icons/fa";

const TestingPage = () => {
  const isEditing = false;

  return (
    <>
      <div className="mx-auto flex min-h-full w-[1024px] flex-col items-center gap-6 p-4">
        <div className="h-16 w-full bg-accent4">top</div>
        <div className="h-full w-full bg-accent2 p-8">
          <div className="h-[100px] min-h-[600px]">Content</div>
        </div>
      </div>
    </>
  );
};

const Wave = () => {
  useEffect(() => {
    const tween = KUTE.fromTo(
      "#first",
      { path: "#first" },
      { path: "#second" },
      { repeat: 999, duration: 3000, yoyo: true }
    );

    tween.start();
  }, []);

  return (
    <section>
      <svg
        id="visual"
        viewBox="0 0 900 600"
        width="900"
        height="600"
        xmlns="http://www.w3.org/2000/svg"
        version="1.1"
      >
        <g>
          <path
            id="first"
            d="M0 495L21.5 487.3C43 479.7 86 464.3 128.8 451.2C171.7 438 214.3 427 257.2 417.7C300 408.3 343 400.7 385.8 416C428.7 431.3 471.3 469.7 514.2 481.7C557 493.7 600 479.3 642.8 471C685.7 462.7 728.3 460.3 771.2 445.7C814 431 857 404 878.5 390.5L900 377L900 601L878.5 601C857 601 814 601 771.2 601C728.3 601 685.7 601 642.8 601C600 601 557 601 514.2 601C471.3 601 428.7 601 385.8 601C343 601 300 601 257.2 601C214.3 601 171.7 601 128.8 601C86 601 43 601 21.5 601L0 601Z"
            fill="#0066FF"
            stroke-linecap="round"
            stroke-linejoin="miter"
          ></path>
        </g>
        <g visibility="hidden">
          <path
            id="second"
            d="M0 410L21.5 408C43 406 86 402 128.8 410C171.7 418 214.3 438 257.2 458.5C300 479 343 500 385.8 505.5C428.7 511 471.3 501 514.2 494.2C557 487.3 600 483.7 642.8 488.7C685.7 493.7 728.3 507.3 771.2 503.2C814 499 857 477 878.5 466L900 455L900 601L878.5 601C857 601 814 601 771.2 601C728.3 601 685.7 601 642.8 601C600 601 557 601 514.2 601C471.3 601 428.7 601 385.8 601C343 601 300 601 257.2 601C214.3 601 171.7 601 128.8 601C86 601 43 601 21.5 601L0 601Z"
            fill="#0066FF"
            stroke-linecap="round"
            stroke-linejoin="miter"
          ></path>
        </g>
      </svg>
    </section>
  );
};

export default TestingPage;
