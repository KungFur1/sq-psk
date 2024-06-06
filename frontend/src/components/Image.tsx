import { useState } from 'react';

const Image = ({ src, placeholder, alt, ...props }) => {
    const [imgSrc, setImgSrc] = useState(src);

    const handleError = () => {
        setImgSrc(placeholder);
    };

    return (
        <div className='img-holder'>
            <img
                src={imgSrc}
                alt={alt}
                onError={handleError}
                {...props}
            />
        </div>
    );
};

export default Image;
